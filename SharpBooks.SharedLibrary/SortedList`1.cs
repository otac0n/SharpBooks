﻿//-----------------------------------------------------------------------
// <copyright file="SortedList`1.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    public class SortedList<T> : IList<T>
    {
        private readonly IComparer<T> comparer;
        private readonly List<T> storage;

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
                throw new ArgumentNullException("comparer");
            }

            this.storage = new List<T>(collection);
            this.storage.Sort(this.comparer);
        }

        public int IndexOf(T item)
        {
            var result = this.storage.BinarySearch(item, this.comparer);

            return result > 0
                ? result
                : -1;
        }

        public void Insert(int index, T item)
        {
            throw new InvalidOperationException();
        }

        public void RemoveAt(int index)
        {
            this.storage.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return this.storage[index];
            }

            set
            {
                throw new InvalidOperationException();
            }
        }

        public void Add(T item)
        {
            var result = this.storage.BinarySearch(item, this.comparer);

            if (result < 0)
            {
                result = ~result;
            }

            this.storage.Insert(result, item);
        }

        public void Clear()
        {
            this.storage.Clear();
        }

        public bool Contains(T item)
        {
            var result = this.storage.BinarySearch(item, this.comparer);

            return result >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.storage.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.storage.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            var result = this.storage.BinarySearch(item, this.comparer);

            if (result > 0)
            {
                this.storage.RemoveAt(result);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.storage.GetEnumerator();
        }
    }
}