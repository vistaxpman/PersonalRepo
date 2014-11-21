using Microsoft.ComplexEventProcessing;
using System;
using System.IO;
using System.Reactive;

namespace StreamInsightConsoleApplication
{
    public class CsvWriter
    {
        public static IObserver<PointEvent<Payload>> Write(string path, bool echoToConsole)
        {
            var writer = GetStringLogObserver(path, false);
            return Observer.Create<PointEvent<Payload>>(
                x =>
                {
                    var str = FormatOutputLine(x);
                    writer.OnNext(str);
                    if (echoToConsole)
                    {
                        Console.WriteLine(str);
                    }
                },
                writer.OnError,
                writer.OnCompleted);
        }

        private static IObserver<string> GetStringLogObserver(string path, bool overwrite)
        {
            var writer = new StreamWriter(path, overwrite);
            return Observer.Create<string>(
                x =>
                {
                    writer.WriteLine(x);
                    writer.Flush();
                },
                err => writer.Dispose(),
                writer.Dispose);
        }

        private static string FormatOutputLine(PointEvent<Payload> evt)
        {
            if (evt.EventKind == EventKind.Insert)
            {
                return string.Format("{0},{1}", evt.StartTime, evt.Payload);
            }
            else
            {
                return evt.StartTime.ToString();
            }
        }
    }
}