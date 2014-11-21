using System;
using System.Collections.Generic;
using System.Threading;

namespace SynchronizedQueueDemo
{
    internal class BlockingQueueWithMonitorAndGuardedRegion<T> : ISynchronizedQueue<T>
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
            return _lock.When(
                () => _queue.Count > 0,
                () => _queue.Dequeue()
            );
        }
    }

    public static class GuardedRegion
    {
        public static T When<T>(this object obj, Func<bool> predicate, Func<T> body)
        {
            lock (obj)
            {
                while (!predicate())
                {
                    Monitor.Wait(obj);
                }
                return body();
            }
        }
    }
}