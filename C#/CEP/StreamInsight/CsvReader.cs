using Microsoft.ComplexEventProcessing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace StreamInsightConsoleApplication
{
    public class CsvReader
    {
        public static IEnumerable<PointEvent<Payload>> Read(string path)
        {
            foreach (var line in File.ReadLines(path))
            {
                var parts = line.Split(new[] { ',' });
                var returnValue = PointEvent.CreateInsert(
                    DateTimeOffset.Parse(parts[0], DateTimeFormatInfo.InvariantInfo),
                    new Payload
                        {
                            X = double.Parse(parts[1], CultureInfo.InvariantCulture),
                            Y = double.Parse(parts[2], CultureInfo.InvariantCulture)
                        }
                );
                yield return returnValue;
            }
        }
    }
}