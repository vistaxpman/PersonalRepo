using System;
using System.Diagnostics;
using System.Threading;

namespace PerformanceCounterDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var categoryName = "RefWiresDemo";
            var counterName = "DemoCounter";
            if (!PerformanceCounterCategory.Exists(categoryName))
            {
                PerformanceCounterCategory.Create(categoryName, "Demo Category of RefWires",
                    PerformanceCounterCategoryType.SingleInstance, counterName,
                    "Democounter of RefWires");
            }

            var counter = new PerformanceCounter(categoryName, counterName, false);
            var random = new Random();
            while (true)
            {
                int randomValue = random.Next(101);
                Console.WriteLine("The random value is {0, 3}", randomValue);
                counter.RawValue = randomValue;
                Thread.Sleep(100);
            }
        }
    }
}
