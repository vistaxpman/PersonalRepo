using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Linq;
using System;
using System.Reactive.Concurrency;

namespace StreamInsightConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var delay = TimeSpan.FromSeconds(.002);
            using (var server = Server.Create("StreamInsightTest"))
            {
                var applicaiton = server.CreateApplication("StreamInsightDemo");
                var input = applicaiton.DefineObservable((DateTimeOffset? _) => CsvReader.Read("sample.csv").RateLimit(delay, Scheduler.ThreadPool));
                var inputStream = input.ToPointStreamable(x => x, AdvanceTimeSettings.UnorderedStartTime(TimeSpan.FromSeconds(15)));
                var query = from x in inputStream.TumblingWindow(TimeSpan.FromSeconds(10))
                            select new Payload { X = x.Avg(p => p.X), Y = x.Avg(p => p.Y) };
                var output = applicaiton.DefineObserver((DateTimeOffset? hwm, int offset) => CsvWriter.Write("output.csv", true));
                query.Bind(output).Run();
                Console.ReadLine();
            }
        }
    }
}