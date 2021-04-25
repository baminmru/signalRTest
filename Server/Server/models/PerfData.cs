using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.models
{

    public class perfdriverinfo
    {
        public System.Guid perfdriverinfoid { get; set; }
        public string name { get; set; }
        public string totalsize { get; set; }
        public string freespace { get; set; }
        public string freeuserspace { get; set; }

        public System.Guid perfdataid { get; set; }
    }

    public class perfdata
    {
        public System.Guid perfdataid { get; set; }

        public perfdata()
        {
            driverdata = new List<perfdriverinfo>();
        }
        public string name { get; set; }
        public string cpu { get; set; }

        public string ramtotal { get; set; }


        public string ip { get; set; }

        public string ramfree { get; set; }

        public List<perfdriverinfo> driverdata;


    }

    public class agentsettings
    {
        public System.Guid agentsettingsid { get; set; }
  
        public  Int32 scaninterval { get; set; }
    }
}
