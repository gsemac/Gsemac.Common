using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public class OrderedDictionary<TKey, TValue> :
        IDictionary<TKey, TValue> {

        // Public members

        public TValue this[TKey key] {
            get => baseDictionary[key];
            set => SetValue(key, value);
        }

        public ICollection<TKey> Keys => new LazyReadOnlyCollection<TKey>(orderedKeys);
        public ICollection<TValue> Values => new LazyReadOnlyCollection<TValue>(orderedKeys.Select(key => baseDictionary[key]));
        public int Count => baseDictionary.Count;
        public bool IsReadOnly => baseDictionary.IsReadOnly;

        public OrderedDictionary() :
            this(EqualityComparer<TKey>.Default) {
        }
        public OrderedDictionary(IDictionary<TKey, TValue> baseDictionary) {

            if (baseDictionary is null)
                throw new ArgumentNullException(nameof(baseDictionary));

            this.baseDictionary = baseDictionary;

        }
        public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) :
            this(collection, EqualityComparer<TKey>.Default) {
        }
        public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) :
            this(comparer) {

            AddRange(collection);

        }
        public OrderedDictionary(IEqualityComparer<TKey> comparer) {

            baseDictionary = new Dictionary<TKey, TValue>(comparer);

        }
        public OrderedDictionary(int capacity) {

            baseDictionary = new Dictionary<TKey, TValue>(capacity);

        }
        public OrderedDictionary(int capacity, IEqualityComparer<TKey> comparer) {

            baseDictionary = new Dictionary<TKey, TValue>(capacity, comparer);

        }

        public void Add(TKey key, TValue value) {

            SetValue(key, value);

        }
        public void Add(KeyValuePair<TKey, TValue> item) {

            SetValue(item.Key, item.Value);

        }
        public void Clear() {

            orderedKeys.Clear();

            baseDictionary.Clear();

        }
        public bool Contains(KeyValuePair<TKey, TValue> item) {

            return baseDictionary.Contains(item);

        }
        public bool ContainsKey(TKey key) {

            return baseDictionary.ContainsKey(key);

        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {

            baseDictionary.ToArray().CopyTo(array, arrayIndex);

        }
        public bool Remove(TKey key) {

            orderedKeys.Remove(key);

            return baseDictionary.Remove(key);

        }
        public bool Remove(KeyValuePair<TKey, TValue> item) {

            return Remove(item.Key);

        }
        public bool TryGetValue(TKey key, out TValue value) {

            return baseDictionary.TryGetValue(key, out value);

        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {

            return baseDictionary.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IList<TKey> orderedKeys = new List<TKey>();
        private readonly IDictionary<TKey, TValue> baseDictionary;

        private void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection) {

            foreach (var pair in collection)
                Add(pair);

        }
        private void SetValue(TKey key, TValue value) {

            Remove(key);

            orderedKeys.Add(key);

            baseDictionary.Add(key, value);

        }

    }

}