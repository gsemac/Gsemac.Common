using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public class OrderedDictionary<TKey, TValue> :
        IDictionary<TKey, TValue> {

        // Public members

        public TValue this[TKey key] {
            get => underlyingDict[key];
            set => SetValue(key, value);
        }

        public ICollection<TKey> Keys => new LazyReadOnlyCollection<TKey>(orderedKeys);
        public ICollection<TValue> Values => new LazyReadOnlyCollection<TValue>(orderedKeys.Select(key => underlyingDict[key]));
        public int Count => underlyingDict.Count;
        public bool IsReadOnly => false;

        public OrderedDictionary() {
        }
        public OrderedDictionary(IDictionary<TKey, TValue> underlyingDictionary) {

            this.underlyingDict = underlyingDictionary;

        }

        public void Add(TKey key, TValue value) {

            SetValue(key, value);

        }
        public void Add(KeyValuePair<TKey, TValue> item) {

            SetValue(item.Key, item.Value);

        }
        public void Clear() {

            orderedKeys.Clear();

            underlyingDict.Clear();

        }
        public bool Contains(KeyValuePair<TKey, TValue> item) {

            return underlyingDict.Contains(item);

        }
        public bool ContainsKey(TKey key) {

            return underlyingDict.ContainsKey(key);

        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {

            underlyingDict.ToArray().CopyTo(array, arrayIndex);

        }
        public bool Remove(TKey key) {

            orderedKeys.Remove(key);

            return underlyingDict.Remove(key);

        }
        public bool Remove(KeyValuePair<TKey, TValue> item) {

            return Remove(item.Key);

        }
        public bool TryGetValue(TKey key, out TValue value) {

            return underlyingDict.TryGetValue(key, out value);

        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {

            return underlyingDict.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IList<TKey> orderedKeys = new List<TKey>();
        private readonly IDictionary<TKey, TValue> underlyingDict = new Dictionary<TKey, TValue>();

        private void SetValue(TKey key, TValue value) {

            if (ContainsKey(key))
                Remove(key);

            orderedKeys.Add(key);

            underlyingDict.Add(key, value);

        }

    }

}