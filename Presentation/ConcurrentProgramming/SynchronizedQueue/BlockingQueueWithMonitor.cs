using System.Collections.Generic;
using System.Threading;

namespace SynchronizedQueueDemo
{
    internal class BlockingQueueWithMonitor<T> : ISynchronizedQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly object _lock = new object();

        public void Enqueue(T item)
        {
            lock (_lock)
            {
                _queue.Enqueue(item);

                // Wakeup any/all waiters
                Monitor.PulseAll(_lock);
            }
        }

        public T Dequeue()
        {
            T item;
            lock (_lock)
            {
                // Loop waiting for condition (queue not empty)
                while (_queue.Count == 0)
                {
                    Monitor.Wait(_lock);
                }

                item = _queue.Dequeue();
            }

            return item;
        }
    }
}