using System.Collections.Generic;
using System.Threading;

namespace SynchronizedQueueDemo
{
    internal class BlockingBoundedQueue<T> : ISynchronizedQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly object _fullEvent = new object();
        private readonly object _emptyEvent = new object();

        private readonly int _capacity;
        private int _fullWaiters = 0;
        private int _emptyWaiters = 0;

        public BlockingBoundedQueue(int capacity)
        {
            _capacity = capacity;
        }

        public void Enqueue(T item)
        {
            lock (_queue)
            {
                while (_queue.Count == _capacity)
                {
                    // The queue is full.
                    _fullWaiters++;
                    try
                    {
                        lock (_fullEvent)
                        {
                            Monitor.Exit(_queue);
                            Monitor.Wait(_fullEvent);
                            Monitor.Enter(_queue);
                        }
                    }
                    finally
                    {
                        _fullWaiters--;
                    }
                }
                _queue.Enqueue(item);
            }

            if (_emptyWaiters > 0)
            {
                lock (_emptyEvent)
                {
                    Monitor.Pulse(_emptyEvent);
                }
            }
        }

        public T Dequeue()
        {
            T item;

            lock (_queue)
            {
                // Empty
                while (_queue.Count == 0)
                {
                    _emptyWaiters++;
                    try
                    {
                        lock (_emptyEvent)
                        {
                            Monitor.Exit(_queue);
                            Monitor.Wait(_emptyEvent);
                            Monitor.Enter(_queue);
                        }
                    }
                    finally
                    {
                        _emptyWaiters--;
                    }
                }
                item = _queue.Dequeue();
            }

            if (_fullWaiters > 0)
            {
                lock (_fullEvent)
                {
                    Monitor.Pulse(_fullEvent);
                }
            }
            return item;
        }
    }
}