using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmicTrading.Common.Utilities
{
    public static class EnumerableExtension
    {
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size = 2000)
        {
            var partition = new List<T>(size);
            var counter = 0;

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    partition.Add(enumerator.Current);
                    counter++;
                    if (counter % size == 0)
                    {
                        yield return partition.ToList();
                        partition.Clear();
                        counter = 0;
                    }
                }

                if (counter != 0)
                    yield return partition;
            }
        }

        public static IEnumerable<DateTime> GetDateRange(this DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("endDate must be greater than or equal to startDate");
            }

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }
    }
}