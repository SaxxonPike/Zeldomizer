using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    /// <summary>
    /// A fixed list of elements.
    /// </summary>
    /// <typeparam name="T">Type of element.</typeparam>
    public abstract class FixedList<T> : IList<T>, IReadOnlyList<T>
    {
        /// <summary>
        /// Get the capacity of the fixed list.
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Initialize a fixed list.
        /// </summary>
        /// <param name="capacity"></param>
        protected FixedList(int capacity)
        {
            Capacity = capacity;
            Count = capacity;
        }

        /// <summary>
        /// Get all items in the fixed list.
        /// </summary>
        private IEnumerable<T> Items =>
            Enumerable.Range(0, Count).Select(i => this[i]);

        /// <summary>
        /// Enumerate all items in the fixed list.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => 
            Items.GetEnumerator();

        /// <summary>
        /// Enumerate all items in the fixed list.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        /// <summary>
        /// Throws an exception. Fixed lists cannot have elements added to them.
        /// </summary>
        public virtual void Add(T item)
        {
            throw new Exception("Can't add to the fixed list.");
        }

        /// <summary>
        /// Set all elements in the list to default. Throws an exception for reference types.
        /// </summary>
        public virtual void Clear()
        {
            if (!typeof(T).IsValueType)
                throw new Exception("Can't clear the fixed list.");

            for (var i = 0; i < Count; i++)
                this[i] = default(T);
        }

        /// <summary>
        /// Returns true if the fixed list contains the specified value, false otherwise.
        /// </summary>
        public bool Contains(T item) => 
            Items.Contains(item);

        /// <summary>
        /// Copy the fixed list's elements to an array.
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            var items = Items.ToArray();
            Array.Copy(items, 0, array, arrayIndex, items.Length);
        }

        /// <summary>
        /// Throws an exception. Fixed lists cannot have items removed from them.
        /// </summary>
        public virtual bool Remove(T item) => 
            throw new Exception("Can't remove from the fixed list.");

        /// <summary>
        /// Get the number of elements in the fixed list.
        /// </summary>
        public virtual int Count { get; }

        /// <summary>
        /// Returns true. Fixed lists cannot be modified.
        /// </summary>
        public virtual bool IsReadOnly => true;

        /// <summary>
        /// Returns the index of the specified item in the list, or -1 if the item was not found.
        /// </summary>
        public int IndexOf(T item) => 
            Array.IndexOf(Items.ToArray(), item);

        /// <summary>
        /// Throws an exception. Items cannot be inserted into the fixed list.
        /// </summary>
        public virtual void Insert(int index, T item) => 
            throw new Exception("Can't insert into the fixed list.");

        /// <summary>
        /// Throws an exception. Items cannot be removed from the fixed list.
        /// </summary>
        public virtual void RemoveAt(int index) => 
            throw new Exception("Can't remove from the fixed list.");

        /// <summary>
        /// Get or set the element at the specified index within the list.
        /// </summary>
        public abstract T this[int index] { get; set; }

        /// <summary>
        /// Get a string representation of the list.
        /// </summary>
        public override string ToString()
        {
            return $"[{string.Join(", ", this.Select(i => i.ToString()))}]";
        }
    }
}
