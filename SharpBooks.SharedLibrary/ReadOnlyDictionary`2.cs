// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Collections;

    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> dictionary;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            this.dictionary = dictionary;
        }

        public int Count => this.dictionary.Count;

        public bool IsReadOnly => true;

        public ICollection<TKey> Keys => this.dictionary.Keys;

        public ICollection<TValue> Values => this.dictionary.Values;

        public TValue this[TKey key]
        {
            get { return this.dictionary[key]; }

            set { throw new NotSupportedException(); }
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException();
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

        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.dictionary).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            throw new NotSupportedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }
    }
}
