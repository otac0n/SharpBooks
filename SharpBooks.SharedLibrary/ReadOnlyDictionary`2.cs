//-----------------------------------------------------------------------
// <copyright file="ReadOnlyBook.cs" company="(none)">
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

    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> dictionary;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            this.dictionary = dictionary;
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException();
        }

        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return this.dictionary.Keys; }
        }

        public bool Remove(TKey key)
        {
            throw new NotSupportedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return this.dictionary.Values; }
        }

        public TValue this[TKey key]
        {
            get { return this.dictionary[key]; }

            set { throw new NotSupportedException(); }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.dictionary).GetEnumerator();
        }
    }
}
