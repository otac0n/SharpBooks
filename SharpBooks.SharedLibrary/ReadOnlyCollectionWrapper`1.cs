// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ReadOnlyCollectionWrapper<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private ICollection<T> collection = null;

        public ReadOnlyCollectionWrapper(ICollection<T> wrappedCollection)
        {
            if (wrappedCollection == null)
            {
                throw new ArgumentNullException("wrappedCollection");
            }

            this.collection = wrappedCollection;
        }

        public int Count
        {
            get { return this.collection.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public void Add(T item)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(T item)
        {
            return this.collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.collection).GetEnumerator();
        }

        public bool Remove(T item)
        {
            throw new InvalidOperationException();
        }
    }
}
