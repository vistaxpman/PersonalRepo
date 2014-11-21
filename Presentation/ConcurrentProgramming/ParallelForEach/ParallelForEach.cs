using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelForEach
{
    internal class ParallelForEach
    {
        private static void Main(string[] args)
        {
            string path = @"C:\WINDOWS";

            //var files = Directory.EnumerateFiles(path).Distinct();
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).Distinct();

            MeasurePerformance(() =>
                Console.WriteLine("GetTotalFileLengthByForEach: {0}.", GetTotalFileLengthByForEach(files)));

            MeasurePerformance(() =>
                Console.WriteLine("GetTotalFileLengthByParallelForEach: {0}.", GetTotalFileLengthByParallelForEach(files)));

            MeasurePerformance(() =>
                Console.WriteLine("GetTotalFileLengthByParallelLinq: {0}.", GetTotalFileLengthByParallelLinq(files)));

            Console.ReadLine();
        }

        private static void MeasurePerformance(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            Console.WriteLine("The action finished within {0} ms.\n", stopwatch.ElapsedMilliseconds);
        }

        private static long GetTotalFileLengthByForEach(IEnumerable<string> files)
        {
            return (from file in files
                    select GetFileLength(file)).Sum();
        }

        private static long GetTotalFileLengthByParallelForEach(IEnumerable<string> files)
        {
            long masterTotal = 0;
            var result = Parallel.ForEach<string, long>(
                files,
                () =>
                {
                    return 0;
                },
                (file, loopState, index, taskLocalTotoal) =>
                {
                    long fileLength = GetFileLength(file);
                    return taskLocalTotoal + fileLength;
                },
                taskLocalTotal =>
                {
                    Interlocked.Add(ref masterTotal, taskLocalTotal);
                });

            return masterTotal;
        }

        private static long GetTotalFileLengthByParallelLinq(IEnumerable<string> files)
        {
            return (from file in files.AsParallel()
                    select GetFileLength(file)).Sum();
        }

        private static long GetFileLength(string file)
        {
            FileStream fileStream = null;
            long fileLength = 0;
            try
            {
                return new FileInfo(file).Length;
            }
            catch (IOException)
            {
                // Do nothing.
            }
            catch (UnauthorizedAccessException)
            {
                // Do nothing.
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
            return fileLength;
        }
    }
}