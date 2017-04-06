using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    public abstract class FixedList<T> : IList<T>, IReadOnlyList<T>
    {
        public int Capacity { get; }

        protected FixedList(int capacity)
        {
            Capacity = capacity;
            Count = capacity;
        }

        private IEnumerable<T> Items
        {
            get
            {
                return Enumerable.Range(0, Count).Select(i => this[i]);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(T item)
        {
            throw new Exception("Can't add to the fixed list.");
        }

        public virtual void Clear()
        {
            throw new Exception("Can't clear the fixed list.");
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var items = Items.ToArray();
            Array.Copy(items, 0, array, arrayIndex, items.Length);
        }

        public virtual bool Remove(T item)
        {
            throw new Exception("Can't remove from the fixed list.");
        }

        public virtual int Count { get; }

        public virtual bool IsReadOnly => true;

        public int IndexOf(T item)
        {
            return Array.IndexOf(Items.ToArray(), item);
        }

        public virtual void Insert(int index, T item)
        {
            throw new Exception("Can't insert into the fixed list.");
        }

        public virtual void RemoveAt(int index)
        {
            throw new Exception("Can't remove from the fixed list.");
        }

        public abstract T this[int index] { get; set; }
    }
}
