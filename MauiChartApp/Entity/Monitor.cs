using MauiCommon.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.Entity
{
    [Table("MonitorData")]
    public class MonitorData : BaseEntity
    {
        public DateTime DateTime {  get; set; }
        public string tag { get; set; }

        public long value { get; set; }
    }
}
