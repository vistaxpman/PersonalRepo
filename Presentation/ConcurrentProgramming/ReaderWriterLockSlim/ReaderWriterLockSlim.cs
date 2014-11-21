using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLockSlimDemo
{
    internal class ReaderWriterLockSlimDemo
    {
        private static void Main(string[] args)
        {
            var collection = new ReaderWriterCollection<int>();
            int value = 0;

            Action writerAction = () =>
                {
                    Interlocked.Increment(ref value);
                    Console.WriteLine("Add:   {0}", value);
                    collection.Add(value);
                };

            Action readerAction = () =>
                {
                    Console.WriteLine("Count: {0}", collection.Count);
                };

            // Reader/Writer : 3/1
            Parallel.For(0, 100, i =>
                {
                    if ((i % 4) != 0)
                    {
                        readerAction();
                    }
                    else
                    {
                        writerAction();
                    }
                }
            );

            Console.WriteLine("Count: {0}", collection.Count);
            Console.ReadLine();
        }
    }
}