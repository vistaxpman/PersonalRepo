using System;

namespace SynchronizedQueueDemo
{
    interface ISynchronizedQueue<T>
    {
        T Dequeue();
        void Enqueue(T item);
    }
}
