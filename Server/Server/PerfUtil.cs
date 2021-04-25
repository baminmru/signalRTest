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
        public static perfdata GetData()
        {
            perfdata pd = new perfdata();
            var rams = new ManagementObjectSearcher("select Capacity from Win32_PhysicalMemory").Get();
            foreach (ManagementObject ram in rams)
            {
                pd.ramtotal = (Int64.Parse(ram["Capacity"].ToString()) / 1024 / 1024).ToString() + "МБ";
                break;
            }

            pd.ip = "";
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
                            if (pd.ip != "")
                                pd.ip += ";";
                            pd.ip += s;
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

            pd.name = Environment.MachineName;

            pd.cpu = pCnt.NextValue().ToString();
            pd.ramfree = mCnt.NextValue().ToString() + "МБ";

            foreach (DriveInfo d in allDrives)
            {
                perfdriverinfo pdf = new perfdriverinfo();

                pdf.name = d.Name;
                try
                {
                    pdf.totalsize = (d.TotalSize / 1024 / 1024).ToString() + "МБ";
                }
                catch (Exception ex)
                {
                    pdf.totalsize = ex.Message;
                }


                try
                {
                    pdf.freespace = (d.TotalFreeSpace / 1024 / 1024).ToString() + "МБ";
                }
                catch (Exception ex)
                {
                    pdf.freespace = ex.Message;
                }

                try
                {
                    pdf.freeuserspace = (d.AvailableFreeSpace / 1024 / 1024).ToString() + "МБ";
                }
                catch (Exception ex)
                {
                    pdf.freeuserspace = ex.Message;
                }




                pd.driverdata.Add(pdf);
            }
            return pd;
        }
    }
}
