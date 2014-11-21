using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace StreamInsightConsoleApplication
{
    public static class EnumerableExtension
    {
        public static IObservable<T> RateLimit<T>(this IEnumerable<T> source, TimeSpan period, IScheduler scheduler)
        {
            return Observable.Using(
                source.GetEnumerator,
                it => Observable.Generate(
                    false,
                    _ => it.MoveNext(),
                    _ => _,
                    _ => it.Current,
                    _ => period,
                    scheduler));
        }
    }
}