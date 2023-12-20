using MauiCommon.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.Entity
{
    [Table("AppData")]
    public class AppValue : BaseEntity
    {
        string Key { get; set; }

        public string Value { get; set; }

        public bool boolBalue { get; set; }

        public long longValue { get; set; }


        public AppValue() { }
    }
}
