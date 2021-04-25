using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using Server.models;
using System.IO;

namespace Server
{
    public class PerfUtil
    {
        public static PerfData GetData()
        {
            PerfData pd = new PerfData();
            var rams = new ManagementObjectSearcher("select Capacity from Win32_PhysicalMemory").Get();
            foreach (ManagementObject ram in rams)
            {
                pd.ramTotal = (Int64.Parse(ram["Capacity"].ToString()) / 1024 / 1024).ToString() + "МБ";
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
                                pd.IP += ";";
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
            pd.ramFree = mCnt.NextValue().ToString() + "МБ";

            foreach (DriveInfo d in allDrives)
            {
                PerfDriverInfo pdf = new PerfDriverInfo();

                pdf.Name = d.Name;
                try
                {
                    pdf.Totalsize = (d.TotalSize / 1024 / 1024).ToString() + "МБ";
                }
                catch (Exception ex)
                {
                    pdf.Totalsize = ex.Message;
                }


                try
                {
                    pdf.freeSpace = (d.TotalFreeSpace / 1024 / 1024).ToString() + "МБ";
                }
                catch (Exception ex)
                {
                    pdf.freeSpace = ex.Message;
                }

                try
                {
                    pdf.freeUserSpace = (d.AvailableFreeSpace / 1024 / 1024).ToString() + "МБ";
                }
                catch (Exception ex)
                {
                    pdf.freeUserSpace = ex.Message;
                }




                pd.driverData.Add(pdf);
            }
            return pd;
        }
    }
}
