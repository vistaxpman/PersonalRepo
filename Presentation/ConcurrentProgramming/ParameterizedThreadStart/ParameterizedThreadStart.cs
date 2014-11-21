using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ParameterizedThreadStart
{
    internal class ParameterizedThreadStart
    {
        private static void Main(string[] args)
        {
            int offset = 1;
            int size = 10000000;

            var stopwatch = Stopwatch.StartNew();
            long sequencSum = SequencSum(offset, size);
            Console.WriteLine("  SequenceSum: {0} in {1} ms", sequencSum, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            long concurrentSum = ConcurrentSum(offset, size);
            Console.WriteLine("ConcurrentSum: {0} in {1} ms", sequencSum, stopwatch.ElapsedMilliseconds);
            Console.ReadLine();
        }

        private static long SequencSum(int offset, int size)
        {
            long sum = 0;
            for (int i = offset; i < offset + size; i++)
            {
                sum += i;
            }
            return sum;
        }

        private static long ConcurrentSum(int offset, int size)
        {
            int threadCount = 10;
            Thread[] threads = new Thread[threadCount];
            long[] result = new long[threadCount];

            // Problems:
            //  1. Cost;
            //  2. Code Locality;
            //  3. Hard with excpetion and cancellation;
            //  4. Difficult to get the result: global variable and synchronization;
            //  5. Block issue and thread efficiency;
            //  6. Mass thread communication.
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(obj =>
                    {
                        int count = (int)obj;
                        int segment = size / threadCount;
                        int upper = Math.Min(size, segment * (count + 1));
                        for (int j = segment * count + offset; j <= upper; j++)
                        {
                            result[count] += j;
                        }
                    });
                threads[i].Start(i);
            }

            foreach (var t in threads)
            {
                t.Join();
            }

            return result.Sum();
        }
    }
}