using System;
using System.Collections.Generic;
using System.Text;

namespace agent
{

    public class PerfDriverInfo
    {
        public string Name { get; set; }
        public string TotalSize { get; set; }
        public string FreeSpace { get; set; }
        public string FreeUserSpace { get; set; }
    }

    public class PerfData
    {

        public PerfData()
        {
            DriverData = new List<PerfDriverInfo>();
        }
        public string Name { get; set; }
        public string CPU { get; set; }

        public string RamTotal { get; set; }


        public string IP { get; set; }

        public string RamFree { get; set; }

        public List<PerfDriverInfo> DriverData;


    }
}
