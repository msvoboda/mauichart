using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiCommon.Model;

namespace MauiCommon.Entity
{
    [Table("Log")]
    public class LogItem : BaseEntity
    {
        public DateTime DateTime { get; set; }

        public string Tag { get; set; }

        public string Type { get; set; }
        public string Message { get; set; }

    }
}
