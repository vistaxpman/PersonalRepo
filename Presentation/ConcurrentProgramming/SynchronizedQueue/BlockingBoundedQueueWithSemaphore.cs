using System.Collections.Generic;
using System.Threading;

namespace SynchronizedQueueDemo
{
    internal class BlockingBoundedQueueWithSemaphore<T> : ISynchronizedQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly Mutex _mutex = new Mutex();
        private readonly SemaphoreSlim _producerSemaphore;
        private readonly SemaphoreSlim _consumerSemaphore;

        public BlockingBoundedQueueWithSemaphore(int capacity)
        {
            _producerSemaphore = new SemaphoreSlim(capacity, capacity);
            _consumerSemaphore = new SemaphoreSlim(0, capacity);
        }

        public void Enqueue(T item)
        {
            _producerSemaphore.Wait();

            _mutex.WaitOne();
            try
            {
                _queue.Enqueue(item);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
            _consumerSemaphore.Release();
        }

        public T Dequeue()
        {
            _consumerSemaphore.Wait();
            _mutex.WaitOne();
            T item;
            try
            {
                item = _queue.Dequeue();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
            _producerSemaphore.Release();
            return item;
        }
    }
}