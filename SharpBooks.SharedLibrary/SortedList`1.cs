// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SortedList<T> : IList<T>
    {
        private readonly List<T> storage;
        private IComparer<T> comparer;

        public SortedList()
            : this(Comparer<T>.Default)
        {
        }

        public SortedList(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                this.comparer = Comparer<T>.Default;
            }
            else
            {
                this.comparer = comparer;
            }

            this.storage = new List<T>();
        }

        public SortedList(IEnumerable<T> collection)
            : this(collection, Comparer<T>.Default)
        {
        }

        public SortedList(IEnumerable<T> collection, IComparer<T> comparer)
            : this(comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            this.storage = new List<T>(collection);
            this.storage.Sort(this.comparer);
        }

        /// <inheritdoc/>
        public int Count => this.storage.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public T this[int index]
        {
            get => this.storage[index];

            set => throw new InvalidOperationException();
        }

        public int Add(T item)
        {
            var result = this.storage.BinarySearch(item, this.comparer);

            if (result < 0)
            {
                result = ~result;
            }

            this.storage.Insert(result, item);

            return result;
        }

        /// <inheritdoc/>
        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        public void AddRange(IEnumerable<T> item)
        {
            this.storage.AddRange(item);
            this.storage.Sort(this.comparer);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.storage.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            var result = this.storage.BinarySearch(item, this.comparer);

            return result >= 0;
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.storage.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this.storage.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.storage.GetEnumerator();
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            var result = this.storage.BinarySearch(item, this.comparer);

            return result >= 0
                ? result
                : -1;
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            throw new InvalidOperationException();
        }

        public int Remove(T item)
        {
            var result = this.storage.BinarySearch(item, this.comparer);

            if (result >= 0)
            {
                this.storage.RemoveAt(result);
            }

            return result;
        }

        /// <inheritdoc/>
        bool ICollection<T>.Remove(T item)
        {
            return this.Remove(item) >= 0;
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            this.storage.RemoveAt(index);
        }

        public void SetComparer(IComparer<T> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            this.storage.Sort(comparer);
        }
    }
}
