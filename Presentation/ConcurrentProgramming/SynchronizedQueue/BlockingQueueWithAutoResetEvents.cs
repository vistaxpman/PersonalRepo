using System.Collections.Generic;
using System.Threading;

namespace SynchronizedQueueDemo
{
    internal class BlockingQueueWithAutoResetEvents<T> : ISynchronizedQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly Mutex _mutex = new Mutex();
        private readonly AutoResetEvent _event = new AutoResetEvent(false);

        public void Enqueue(T item)
        {
            _mutex.WaitOne();

            try
            {
                _queue.Enqueue(item);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
            _event.Set();
        }

        public T Dequeue()
        {
            _mutex.WaitOne();
            bool taken = true;
            T item;
            try
            {
                while (_queue.Count == 0)
                {
                    taken = false;

                    // MAY CAUSE DEADLOCK BY ALTERTABLE WAIT...
                    WaitHandle.SignalAndWait(_mutex, _event);
                    _mutex.WaitOne();
                    taken = true;
                }
                item = _queue.Dequeue();
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