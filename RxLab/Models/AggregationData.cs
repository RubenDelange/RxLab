using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxLab.Models
{
    public class AggregationData
    {
        public int Count { get; set; }
        public DateTime? FirstEntry { get; set; }
        public DateTime? LastEntry { get; set; }

        public AggregationData(DateTime firstEntry, DateTime lastEntry, int count)
        {
            FirstEntry = firstEntry;
            LastEntry = lastEntry;
            Count = count;
        }
    }
}
