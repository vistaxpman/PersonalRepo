using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ConcurrentQueue
{
    internal class ConcurrentQueue
    {
        private static void Main(string[] args)
        {
            var collection = new BlockingCollection<Int32>(new ConcurrentQueue<Int32>());

            // A thread pool thread will do the consuming
            ThreadPool.QueueUserWorkItem(ConsumeItems, collection);

            for (int item = 0; item < 100; item++)
            {
                Console.WriteLine("Producing: {0, 2}", item);
                collection.Add(item);
            }

            // Tell the consuming thread(s) that no more items will be added to the collection
            collection.CompleteAdding();

            while (!collection.IsCompleted) { }

            Console.ReadKey();
        }

        private static void ConsumeItems(object o)
        {
            var collection = (BlockingCollection<Int32>)o;

            // Block until an item shows up, then process it
            foreach (var item in collection.GetConsumingEnumerable())
            {
                Console.WriteLine("Consuming: {0, 2}", item);
            }

            // The collection is empty and no more items are going into it
            Console.WriteLine("All items have been consumed");
        }
    }
}