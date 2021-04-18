using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public class NameValueCollection :
        INameValueCollection {

        public string this[string key] {
            get => string.Join(",", underlyingDict[key]);
            set => underlyingDict.Add(key, new List<string>(new[] { value }));
        }

        public ICollection<string> Keys => underlyingDict.Keys;
        public ICollection<string> Values => new LazyReadOnlyCollection<string>(underlyingDict.Keys.SelectMany(key => underlyingDict[key]));
        public int Count => underlyingDict.Keys.Sum(key => underlyingDict[key].Count);
        public bool IsReadOnly => underlyingDict.IsReadOnly;

        public NameValueCollection() {

            underlyingDict = new MultiDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        }
        public NameValueCollection(int capacity) {

            underlyingDict = new MultiDictionary<string, string>(capacity);

        }
        public NameValueCollection(IEqualityComparer<string> comparer) {

            underlyingDict = new MultiDictionary<string, string>(comparer);

        }
        public NameValueCollection(int capacity, IEqualityComparer<string> comparer) {

            underlyingDict = new MultiDictionary<string, string>(capacity, comparer);

        }

        public void Add(string key, string value) {

            underlyingDict.Add(key, value);

        }
        public void Add(KeyValuePair<string, string> item) {

            underlyingDict.Add(item);

        }
        public IEnumerable<string> GetValues(string key) {

            return underlyingDict[key];

        }

        public void Clear() {

            underlyingDict.Clear();

        }

        public bool Contains(KeyValuePair<string, string> item) {

            return underlyingDict.Contains(item);

        }
        public bool ContainsKey(string key) {

            return underlyingDict.ContainsKey(key);

        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {

            underlyingDict.CopyTo(array, arrayIndex);

        }

        public bool Remove(string key) {

            return underlyingDict.Remove(key);

        }
        public bool Remove(KeyValuePair<string, string> item) {

            return underlyingDict.Remove(item);

        }
        public bool TryGetValue(string key, out string value) {

            return underlyingDict.TryGetValue(key, out value);

        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {

            return underlyingDict.Keys.SelectMany(key => underlyingDict[key].Select(value => new KeyValuePair<string, string>(key, value)))
                .GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly MultiDictionary<string, string> underlyingDict;

    }

}