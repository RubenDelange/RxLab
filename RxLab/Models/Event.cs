using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tx.Windows;

namespace RxLab.Models
{
    public class Event
    {
        public EventRecord Record { get; set; }
        public AggregationData AggregationData { get; set; }
        public List<PerformanceSample> PerformanceSamples { get; set; }

        public Event()
        {
            PerformanceSamples = new List<PerformanceSample>();
        }
    }
}
