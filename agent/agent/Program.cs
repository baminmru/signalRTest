using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Management;
using System.Threading;


namespace agent
{
    class Program
    {

        private static SignalRConnection myHub = null;


        private static PerfData GetData()
        {
            PerfData pd = new PerfData();
            var rams = new ManagementObjectSearcher("select Capacity from Win32_PhysicalMemory").Get();
            foreach (ManagementObject ram in rams)
            {
                pd.RamTotal = (Int64.Parse(ram["Capacity"].ToString()) / 1024 / 1024).ToString() + "МБ";
                break;
            }

            pd.IP = "";
            var nets = new ManagementObjectSearcher("select * from Win32_NetworkAdapterConfiguration").Get();
            foreach (ManagementObject net in nets)
            {
                try
                {
                    var prop = net["IPAddress"];
                    if (prop != null)
                    {
                        foreach (string s in (string[])(prop))
                        {
                            if (pd.IP != "")
                                pd.IP += "; ";
                            pd.IP += s;
                        }
                    }
                }
                catch
                {
                }

            }

            PerformanceCounter pCnt = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter mCnt = new PerformanceCounter("Memory", "Available MBytes");

            //PerformanceCounter mCnt = new PerformanceCounter("Память", "Доступно МБ");
            //PerformanceCounter pCnt = new PerformanceCounter("Процессор", "% загруженности процессора", "_Total");


            DriveInfo[] allDrives = DriveInfo.GetDrives();

            pd.Name = Environment.MachineName;

            pd.CPU = pCnt.NextValue().ToString();
            pd.RamFree = mCnt.NextValue().ToString() + "МБ";

            foreach (DriveInfo d in allDrives)
            {
                PerfDriverInfo pdf = new PerfDriverInfo();

                pdf.Name = d.Name;
                try
                {
                    pdf.TotalSize = (d.TotalSize / 1024 / 1024).ToString() + "МБ";
                }
                catch (Exception ex)
                {
                    pdf.TotalSize = ex.Message;
                }


                try
                {
                    pdf.FreeSpace = (d.TotalFreeSpace / 1024 / 1024).ToString() + "МБ";
                }
                catch (Exception ex)
                {
                    pdf.TotalSize = ex.Message;
                }

                try
                {
                    pdf.FreeUserSpace = (d.AvailableFreeSpace / 1024 / 1024).ToString() + "МБ";
                }
                catch (Exception ex)
                {
                    pdf.TotalSize = ex.Message;
                }




                pd.DriverData.Add(pdf);
            }
            return pd;
        }

        public static void Main()
        {
            myHub = new SignalRConnection();

            do
            {
                var pd = GetData();
                var msg = JsonConvert.SerializeObject(pd);
                Console.WriteLine("Send to server:" + msg);
                myHub.Send("http://localhost:30809/agent", msg);
                Thread.Sleep(myHub.Interval * 1000);
            } while (true);
        }
    }
}

