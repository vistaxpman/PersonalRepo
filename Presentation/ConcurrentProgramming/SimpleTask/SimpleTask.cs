using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleTask
{
    internal class SimpleTask
    {
        private static void Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            int offset = 1;

            // The value will cause overflowed.
            // int size = int.MaxValue;

            // The normal value.
            int size = 10000;

            var stopwatch = Stopwatch.StartNew();
            var tasks = ConcurrentSumByTask(offset, size, token);

            Console.ReadLine();
            tokenSource.Cancel();

            long concurrentSum = 0;
            try
            {
                concurrentSum = tasks.Select(t => t.Result).Sum();
            }
            catch (AggregateException ex)
            {
                try
                {
                    ex.Handle(x => x is OperationCanceledException);
                    Console.WriteLine("The action is canceled.");
                }
                catch (AggregateException exception)
                {
                    exception.Handle(x => x is OverflowException);
                    Console.WriteLine("The action is overflowed.");
                }
            }
            Console.WriteLine("Sum: {0}.", concurrentSum, stopwatch.ElapsedMilliseconds);
        }

        private static IEnumerable<Task<int>> ConcurrentSumByTask(int offset, int size, CancellationToken token)
        {
            int taskCount = 5;

            var tasks = from i in Enumerable.Range(0, taskCount)
                        select Task.Factory.StartNew<int>(obj =>
                            {
                                int count = (int)obj;
                                int segment = size / taskCount;
                                int upper = Math.Min(size, segment * (count + 1));
                                int result = 0;
                                for (int j = segment * count + offset; j <= upper; j++)
                                {
                                    token.ThrowIfCancellationRequested();
                                    checked { result += j; }
                                }
                                Thread.Sleep(1000);
                                Console.WriteLine("Task {0} is done", count);
                                return result;
                            }, i, token);

            return tasks.ToArray();
        }
    }
}