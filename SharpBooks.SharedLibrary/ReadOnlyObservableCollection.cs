using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections;

namespace SharpBooks
{
    public class ReadOnlyObservableCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, INotifyCollectionChanged
    {
        ICollection<T> collection = null;

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

            wrappedCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(collection_CollectionChanged);
        }

        void collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(sender, e);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

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

        public int Count
        {
            get { return this.collection.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(T item)
        {
            throw new InvalidOperationException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
