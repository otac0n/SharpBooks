//-----------------------------------------------------------------------
// <copyright file="ReadOnlyObservableCollection.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public class ReadOnlyObservableCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, INotifyCollectionChanged
    {
        private ICollection<T> collection = null;

        public ReadOnlyObservableCollection(INotifyCollectionChanged wrappedCollection)
        {
            if (wrappedCollection == null)
            {
                throw new ArgumentNullException("wrappedCollection");
            }

            this.collection = wrappedCollection as ICollection<T>;

            if (this.collection == null)
            {
                throw new ArgumentException("The wrapped collection must implement the ICollection<T> interface.", "wrappedCollection");
            }

            wrappedCollection.CollectionChanged += this.Collection_CollectionChanged;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Count
        {
            get { return this.collection.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.collection).GetEnumerator();
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

        public bool Remove(T item)
        {
            throw new InvalidOperationException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(sender, e);
            }
        }
    }
}
