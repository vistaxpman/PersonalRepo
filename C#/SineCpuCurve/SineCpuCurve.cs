using System;
using System.Diagnostics;
using System.Threading;

public sealed class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Program SIN");
        Console.WriteLine("Making the Windows Task Manager show Sine Wave Pattern in CPU usage");
        Console.WriteLine("Now look at your task manager cpu usage!");

        int numOfCPUs = Environment.ProcessorCount;
        for (int i = 0; i < numOfCPUs; ++i)
        {
            ThreadPool.QueueUserWorkItem(SinWave);
        }
        Thread.Sleep(Timeout.Infinite);
    }

    private static void SinWave(object dummy)
    {
        while (true)
        {
            for (double i = 0.0; i < 2 * Math.PI; i += 0.1)
            {
                Compute(500, Math.Sin(i) / 2.0 + 0.5);
            }
        }
    }

    private static void Compute(long time, double percent)
    {
        long runTime = (long)(time * percent);
        long sleepTime = time - runTime;
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        while (stopWatch.ElapsedMilliseconds - runTime < 0)
        {
            // Spin the CPU. Just doing nothing is OK
        }
        stopWatch.Stop();
        stopWatch.Reset();
        Thread.Sleep((int)sleepTime);
    }
}