using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxLab.Helpers
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// This extension method is similar to CombineLatest, but instead of pushing a combined result
        /// to the result stream when either of the two source streams receives an event, it only pushes to 
        /// the result stream when the primary source does. A selector function must be provided which is
        /// given the current primary event and the last secondary event.
        /// </summary>
        public static IObservable<TResult> ComposeLatest<TPrimary, TSecondary, TResult>(
                this IObservable<TPrimary> primarySource,
                IObservable<TSecondary> secondarySource,
                Func<TPrimary, TSecondary, TResult> selector)
        {
            return Observable.Defer(() =>
            {
                var lastIndex = -1;
                return primarySource.Select(Tuple.Create<TPrimary, int>)
                                    .CombineLatest(
                                                    secondarySource,
                                                    (primary, secondary) =>
                                                    new
                                                    {
                                                        Index = primary.Item2,
                                                        Primary = primary.Item1,
                                                        Secondary = secondary
                                                    })
                                    .Where(x => x.Index != lastIndex)
                                    .Do(x => lastIndex = x.Index)
                                    .Select(x => selector(x.Primary, x.Secondary));
            });
        }
    }
}
