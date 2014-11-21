using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Granularity
{
    internal class Granularity
    {
        private static void Main(string[] args)
        {
            int upper = 2000000;
            long checkedSum = 1999999000000;
            var array = Enumerable.Range(0, upper).ToArray();

            long sum = 0;
            var stopwatch = Stopwatch.StartNew();
            foreach (var i in array)
            {
                sum += i;
            }
            var sequentialTime = stopwatch.ElapsedMilliseconds;
            Debug.Assert(sum == checkedSum);
            Console.WriteLine("      Sequential: {0, 4} ms", sequentialTime);

            foreach (int j in new int[] { 2, 4, 5, 10, 20, 40, 50, 100, 200, 400, 500, 1000, 2000, 4000, 5000, 10000,
                20000, 40000, 50000, 100000, 200000, 400000, 500000, 1000000})
            {
                sum = 0;
                stopwatch.Restart();
                Parallel.For(0, j, i =>
                    {
                        int l = upper / j * i;
                        int u = upper / j * (i + 1);

                        // Notice: a local variable here can prompt the performance.
                        long localSum = 0;
                        for (int k = l; k < u; k++)
                        {
                            localSum += k;
                        }
                        Interlocked.Add(ref sum, localSum);
                    }
                );
                var goodParallelTime = stopwatch.ElapsedMilliseconds;
                Debug.Assert(sum == checkedSum);
                Console.WriteLine("{0, 7} parallel: {1, 4} ms", j, goodParallelTime);
            }

            sum = 0;
            stopwatch.Restart();
            Parallel.For(0, upper, i => Interlocked.Add(ref sum, i));
            var fullParallelTime = stopwatch.ElapsedMilliseconds;
            Debug.Assert(sum == checkedSum);
            Console.WriteLine("   Full parallel: {0, 4} ms", fullParallelTime);

            foreach (int j in new int[] { 2, 4, 5, 10, 20, 40, 50, 100, 200, 400, 500, 1000, 2000, 4000, 5000, 10000,
                20000, 40000, 50000, 100000, 200000, 400000, 500000, 1000000})
            {
                sum = 0;
                stopwatch.Restart();
                Parallel.For(0, j, i =>
                    {
                        int l = upper / j * i;
                        int u = upper / j * (i + 1);
                        for (int k = l; k < u; k++)
                        {
                            Interlocked.Add(ref sum, k);
                        }
                    }
                );
                var badParallelTime = stopwatch.ElapsedMilliseconds;
                Debug.Assert(sum == checkedSum);
                Console.WriteLine("{0, 7} parallel: {1, 4} ms [FalseSharing]", j, badParallelTime);
            }

            Console.ReadLine();
        }
    }
}