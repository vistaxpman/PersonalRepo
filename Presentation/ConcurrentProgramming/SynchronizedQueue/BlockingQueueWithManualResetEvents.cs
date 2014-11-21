using System.Collections.Generic;
using System.Threading;

namespace SynchronizedQueueDemo
{
    internal class BlockingQueueWithManualResetEvents<T> : ISynchronizedQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly Mutex _mutex = new Mutex();
        private readonly ManualResetEvent _event = new ManualResetEvent(false);

        public void Enqueue(T item)
        {
            _mutex.WaitOne();
            try
            {
                _queue.Enqueue(item);
                if (_queue.Count == 1)
                {
                    _event.Set();
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public T Dequeue()
        {
            _mutex.WaitOne();

            T item;
            bool taken = true;

            try
            {
                while (_queue.Count == 0)
                {
                    taken = false;
                    _mutex.ReleaseMutex();
                    _event.WaitOne();
                    _mutex.WaitOne();
                    taken = true;
                }

                item = _queue.Dequeue();
                if (_queue.Count == 0)
                {
                    _event.Reset();
                }
            }
            finally
            {
                if (taken)
                {
                    _mutex.ReleaseMutex();
                }
            }

            return item;
        }
    }
}