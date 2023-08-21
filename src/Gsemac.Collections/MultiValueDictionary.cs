using Gsemac.Collections.Extensions;
using Gsemac.Collections.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if ENABLE_POLYFILLS
using Gsemac.Polyfills.System.Collections.Generic;
using Gsemac.Polyfills.System.Collections.ObjectModel;
#else
using System.Collections.ObjectModel;
#endif

namespace Gsemac.Collections {

    public class MultiValueDictionary<TKey, TValue> :
        IReadOnlyDictionary<TKey, IReadOnlyCollection<TValue>> {

        // Public members

        public IReadOnlyCollection<TValue> this[TKey key] => GetValue(key);

        public int Count => underlyingDict.Count;
        public bool IsReadOnly => underlyingDict.IsReadOnly;

        public IEnumerable<TKey> Keys => GetKeys();
        public IEnumerable<IReadOnlyCollection<TValue>> Values => GetValues();

        public MultiValueDictionary() {

            underlyingDict = new Dictionary<TKey, IList<TValue>>();

        }
        public MultiValueDictionary(int capacity) {

            underlyingDict = new Dictionary<TKey, IList<TValue>>(capacity);

        }
        public MultiValueDictionary(IEqualityComparer<TKey> comparer) {

            underlyingDict = new Dictionary<TKey, IList<TValue>>(comparer);

        }
        public MultiValueDictionary(int capacity, IEqualityComparer<TKey> comparer) {

            underlyingDict = new Dictionary<TKey, IList<TValue>>(capacity, comparer);

        }

        public void Add(TKey key, TValue value) {

            if (underlyingDict.TryGetValue(key, out IList<TValue> list)) {

                list.Add(value);

            }
            else {

                underlyingDict[key] = new List<TValue> {
                    value
                };

            }

        }
        public void AddRange(TKey key, IEnumerable<TValue> values) {

            if (underlyingDict.TryGetValue(key, out IList<TValue> list)) {

                list.AddRange(values);

            }
            else {

                underlyingDict[key] = new List<TValue>(values);

            }

        }
        public void Clear() {

            underlyingDict.Clear();

        }
        public bool Contains(TKey key, TValue value) {

            return underlyingDict.TryGetValue(key, out IList<TValue> list) &&
                list.Contains(value);

        }
        public bool ContainsKey(TKey key) {

            return underlyingDict.ContainsKey(key);

        }
        public bool ContainsValue(TValue value) {

            return underlyingDict.Values.Any(list => list.Contains(value));

        }
        public bool Remove(TKey key, TValue value) {

            bool removed = false;

            if (underlyingDict.TryGetValue(key, out IList<TValue> list)) {

                removed = list.Remove(value);

                if (list.Count <= 0)
                    Remove(key);

            }

            return removed;

        }
        public bool Remove(TKey key) {

            return underlyingDict.Remove(key);

        }

        public IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> GetEnumerator() {

            return underlyingDict.Select(pair => new KeyValuePair<TKey, IReadOnlyCollection<TValue>>(pair.Key, CreateReadOnlyCollection(pair.Value)))
                .GetEnumerator();

        }

        public bool TryGetValue(TKey key, out IReadOnlyCollection<TValue> value) {

            if (underlyingDict.TryGetValue(key, out IList<TValue> list)) {

                value = CreateReadOnlyCollection(list);

                return true;

            }
            else {

                value = null;

                return false;

            }

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDictionary<TKey, IList<TValue>> underlyingDict;

        private IReadOnlyCollection<TValue> GetValue(TKey key) {

            if (underlyingDict.TryGetValue(key, out IList<TValue> value))
                return CreateReadOnlyCollection(value);

            throw new KeyNotFoundException(ExceptionMessages.KeyNotFound);

        }
        private ICollection<TKey> GetKeys() {

            return underlyingDict.Keys;

        }
        private ICollection<IReadOnlyCollection<TValue>> GetValues() {

            return underlyingDict.Values
                .Select(list => CreateReadOnlyCollection(list))
                .ToList();

        }
        private IReadOnlyCollection<TValue> CreateReadOnlyCollection(IList<TValue> list) {

            return new ReadOnlyCollection<TValue>(list);

        }

    }

}