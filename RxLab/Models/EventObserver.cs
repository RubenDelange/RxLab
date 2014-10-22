using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxLab.Models
{
    public class EventObserver : IObserver<IList<Event>>
    {
        public void OnNext(IList<Event> eventRecords)
        {
            if (eventRecords.Count == 0)
                return;

            var beforeCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                
                var aggregatedEvents = AggregateEvents(eventRecords);

                foreach (var evt in aggregatedEvents)
                {
                    Console.WriteLine(evt.Record.FormatDescription());
                }
            }
            catch (Exception exception)
            {
                this.OnError(exception);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = beforeCulture;
            }
        }

        private IEnumerable<Event> AggregateEvents(IEnumerable<Event> events)
        {
            return events.GroupBy(evt => evt.Record.FormatDescription())
                         .Select(group =>
                         {
                             var first = group.First();
                             var last = group.Last();
                             first.AggregationData = new AggregationData(
                                 first.Record.TimeCreated ?? DateTime.MinValue,
                                 last.Record.TimeCreated ?? DateTime.MinValue,
                                 group.Count());
                             return first;
                         });
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
