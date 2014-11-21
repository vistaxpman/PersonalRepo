using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadCost
{
    internal class ThreadCost
    {
        private static void Main(string[] args)
        {
            var M = Math.Pow(2, 20);

            using (var manualEvent = new ManualResetEventSlim(false))
            {
                Console.WriteLine("Check the Cost of Thread Creation.");

                Process process = Process.GetCurrentProcess();
                var initialMemorySize = process.VirtualMemorySize64;
                Console.WriteLine("{0,35}: {1:F2} M", "Initial Virtual Memory", initialMemorySize / M);
                var initialHandleCount = process.HandleCount;
                Console.WriteLine("{0,35}: {1}", "Initial Handles", initialHandleCount);

                var loopCount = 1000;
                Console.Write("\nCreate {0} threads... ", loopCount);

                var stopwatcher = Stopwatch.StartNew();
                for (int i = 0; i < loopCount; ++i)
                {
                    new Thread(() => { manualEvent.Wait(); }) { IsBackground = true }.Start();
                }
                Console.WriteLine("Done");

                var elapsedMilliseconds = stopwatcher.ElapsedMilliseconds;
                Console.WriteLine("{0,35}: {1} ms", string.Format("\nCreation time for total {0} thread", loopCount), elapsedMilliseconds);
                Console.WriteLine("{0,35}: {1} ms", "Creation time for each thread", elapsedMilliseconds / loopCount);

                process = Process.GetCurrentProcess();
                var newMemorySize = process.VirtualMemorySize64;
                Console.WriteLine("\n{0,35}: {1:F2} M", "Virtual Memory", newMemorySize / M);
                Console.WriteLine("{0,35}: {1:F2} M", "Memory for each thread", (newMemorySize - initialMemorySize) / (M * loopCount));

                var newHandleCount = process.HandleCount;
                Console.WriteLine("\n{0,35}: {1}", " Handles", newHandleCount);
                Console.WriteLine("{0,35}: {1}", "Handles for each thread", (int)((double)newHandleCount - initialHandleCount) / (loopCount));

                manualEvent.Set();
                Console.ReadLine();
            }
        }
    }
}