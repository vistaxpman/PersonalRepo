using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace ReaderWriterLockSlimDemo
{
    // Notice: ReaderWriterLock is slower than Monitor. It can promt the performance only when many readers with
    // less writer.
    public class ReaderWriterCollection<T> : ICollection<T>
    {
        // Notice: NoRecursion has better performance than Recursion.
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        private Collection<T> _collection = new Collection<T>();

        public void Add(T item)
        {
            _lock.EnterWriteLock();
            _collection.Add(item);
            _lock.ExitWriteLock();
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            _collection.Clear();
            _lock.ExitWriteLock();
        }

        public bool Contains(T item)
        {
            _lock.EnterReadLock();
            var temp = _collection.Contains(item);
            _lock.ExitReadLock();
            return temp;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _lock.EnterReadLock();
            _collection.CopyTo(array, arrayIndex);
            _lock.ExitReadLock();
        }

        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                var temp = _collection.Count;
                _lock.ExitReadLock();
                return temp;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            _lock.EnterWriteLock();
            var temp = _collection.Remove(item);
            _lock.ExitWriteLock();
            return temp;
        }

        public IEnumerator<T> GetEnumerator()
        {
            _lock.EnterReadLock();
            var temp = _collection.GetEnumerator();
            _lock.ExitReadLock();
            return temp;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            _lock.EnterReadLock();
            var temp = _collection.GetEnumerator();
            _lock.ExitReadLock();
            return temp;
        }
    }
}