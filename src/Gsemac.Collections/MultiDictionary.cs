using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Collections {

    public class MultiDictionary<TKey, TValue> :
        IDictionary<TKey, ICollection<TValue>> {

        // Public members

        public ICollection<TValue> this[TKey key] {
            get {

                if (underlyingDict.TryGetValue(key, out ICollection<TValue> items))
                    return items;

                // If the collection is not found in the dictionary, instantiate an empty collection and associate it with the key.

                List<TValue> newCollection = new List<TValue>();

                underlyingDict[key] = newCollection;

                return newCollection;

            }
            set => underlyingDict[key] = value;
        }
        public ICollection<TKey> Keys => underlyingDict.Keys;
        public ICollection<ICollection<TValue>> Values => underlyingDict.Values;
        public int Count => underlyingDict.Count;
        public bool IsReadOnly => underlyingDict.IsReadOnly;

        public void Add(TKey key, ICollection<TValue> value) {

            underlyingDict.Add(key, value);

        }
        public void Add(TKey key, TValue value) {

            ICollection<TValue> existingItems = this[key];

            existingItems.Add(value);

        }
        public void Add(KeyValuePair<TKey, ICollection<TValue>> item) {

            underlyingDict.Add(item);

        }
        public void Clear() {

            underlyingDict.Clear();

        }
        public bool Contains(KeyValuePair<TKey, ICollection<TValue>> item) {

            return underlyingDict.Contains(item);

        }
        public bool ContainsKey(TKey key) {

            return underlyingDict.ContainsKey(key);

        }
        public void CopyTo(KeyValuePair<TKey, ICollection<TValue>>[] array, int arrayIndex) {

            underlyingDict.CopyTo(array, arrayIndex);

        }

        public bool Remove(TKey key) {

            return underlyingDict.Remove(key);

        }
        public bool Remove(KeyValuePair<TKey, ICollection<TValue>> item) {

            return underlyingDict.Remove(item);

        }
        public bool TryGetValue(TKey key, out ICollection<TValue> value) {

            return underlyingDict.TryGetValue(key, out value);

        }

        public IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator() {

            return underlyingDict.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDictionary<TKey, ICollection<TValue>> underlyingDict = new Dictionary<TKey, ICollection<TValue>>();

    }

}