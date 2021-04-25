using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.models
{

    [Table("perfdriverinfo")]
    public class PerfDriverInfo
    {
        [Column("perfdriverinfoid")]
        public System.Guid PerfDriverInfoid { get; set; }
        [Column("name")]
        public string Name { get; set; }

        [Column("totalsize")]
        public string Totalsize { get; set; }
        [Column("freespace")]
        public string freeSpace { get; set; }
        [Column("freeuserspace")]
        public string freeUserSpace { get; set; }

        [Column("perfdataid")]
        public System.Guid perfDataid { get; set; }
    }

    [Table("perfdata")]
    public class PerfData
    {
        [Column("perfdataid")]
        public System.Guid PerfDataID { get; set; }

        public PerfData()
        {
            driverData = new List<PerfDriverInfo>();
        }

        [Column("name")]
        public string Name { get; set; }

        [Column("cpu")]
        public string CPU { get; set; }

        [Column("ramtotal")]
        public string ramTotal { get; set; }

        [Column("ip")]
        public string IP { get; set; }

        [Column("ramfree")]
        public string ramFree { get; set; }

        public List<PerfDriverInfo> driverData;


    }


    [Table("agentsettings")]
    public class AgentSettings
    {
        [Key]
        [Column("name")]
        public String Name { get; set; }

        [Column("value")]
        public  Int32 Value { get; set; }
    }
}
