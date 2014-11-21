using System;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizedQueueDemo
{
    internal class SynchronizedQueueDemo
    {
        private static void Main(string[] args)
        {
            ISynchronizedQueue<int> queue = new BlockingQueueWithMonitor<int>();
            int size = 100;
            var producerCountdownEvent = new CountdownEvent(size);
            var consumerCountdownEvent = new CountdownEvent(size);
            int producerCounter = 0;
            Action<object> producerAction = obj =>
                {
                    while (producerCounter < 99)
                    {
                        queue.Enqueue(producerCounter);
                        Console.WriteLine("Producer {0}: {1, 2} Enqueue", obj, producerCounter);
                        Interlocked.Increment(ref producerCounter);
                        producerCountdownEvent.Signal();
                    }
                };

            Action<object> consumerAction = obj =>
            {
                while (true)
                {
                    int value = queue.Dequeue();
                    Console.WriteLine("Consumer {0}: {1, 2} Dnqueue", obj, value);
                    consumerCountdownEvent.Signal();
                }
            };

            Task.Factory.StartNew(producerAction, 0);
            Task.Factory.StartNew(producerAction, 1);
            Task.Factory.StartNew(consumerAction, 0);
            Task.Factory.StartNew(consumerAction, 1);

            producerCountdownEvent.Wait();
            consumerCountdownEvent.Wait();
            Console.ReadLine();
        }
    }
}