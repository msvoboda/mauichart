using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCommon.Model
{

    public class PageList<T> : List<T>
    {
        public PageList(List<T> items, int start, int total)
        { 
            AddRange(items);            
            IndexFrom = start;
            TotalCount = total;
        }
        public PageList() { }

        public int TotalCount { get; set; }

        public int IndexFrom { get; set; }
    }
}
