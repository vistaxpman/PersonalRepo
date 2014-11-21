using System;
using System.Diagnostics;
using System.Threading;

namespace QueryUserWorkItem
{
    internal class QueryUserWorkItem
    {
        private static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            // ThreadPool is introduced to solve the cost issue.
            for (int i = 0; i < 2000; i++)
            {
                ThreadPool.QueueUserWorkItem(obj =>
                    {
                        var tokenSource = (CancellationTokenSource)obj;
                        for (int j = 0; j < 10; j++)
                        {
                            if (tokenSource.IsCancellationRequested)
                            {
                                break;
                            }
                            int threadCount = Process.GetCurrentProcess().Threads.Count;
                            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                            Console.WriteLine("[{0:5}]:There are {1} threads in the process.", managedThreadId, threadCount);
                        }

                        string result;
                        if (tokenSource.IsCancellationRequested)
                        {
                            result = "cancelled";
                        }
                        else
                        {
                            result = "finished";
                        }
                        Console.WriteLine("The action is {0}.", result);
                    }, cancellationTokenSource);
            }
            Console.ReadLine();
            cancellationTokenSource.Cancel();
        }
    }
}