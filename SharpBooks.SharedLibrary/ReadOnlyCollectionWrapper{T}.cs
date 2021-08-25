// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ReadOnlyCollectionWrapper<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly ICollection<T> collection;

        public ReadOnlyCollectionWrapper(ICollection<T> wrappedCollection)
        {
            this.collection = wrappedCollection ?? throw new ArgumentNullException(nameof(wrappedCollection));
        }

        /// <inheritdoc/>
        public int Count => this.collection.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => true;

        /// <inheritdoc/>
        public void Add(T item)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            return this.collection.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.collection.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.collection).GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            throw new InvalidOperationException();
        }
    }
}
