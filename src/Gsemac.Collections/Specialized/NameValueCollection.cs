using Gsemac.Collections.Extensions;
using Gsemac.Collections.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if ENABLE_POLYFILLS
using Gsemac.Polyfills.System.Collections.Generic;
#endif

namespace Gsemac.Collections.Specialized {

    public class NameValueCollection :
        INameValueCollection {

        public string this[string key] {
            get => Get(key);
            set => Set(key, value);
        }

        public ICollection<string> Keys => new LazyReadOnlyCollection<string>(underlyingDict.Keys);
        public ICollection<string> Values => new LazyReadOnlyCollection<string>(underlyingDict.Keys.SelectMany(key => underlyingDict[key]));
        public int Count => underlyingDict.Keys.Count();
        public bool IsReadOnly => underlyingDict.IsReadOnly;

        public NameValueCollection() {

            underlyingDict = new MultiValueDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        }
        public NameValueCollection(int capacity) {

            underlyingDict = new MultiValueDictionary<string, string>(capacity);

        }
        public NameValueCollection(IEqualityComparer<string> comparer) {

            underlyingDict = new MultiValueDictionary<string, string>(comparer);

        }
        public NameValueCollection(int capacity, IEqualityComparer<string> comparer) {

            underlyingDict = new MultiValueDictionary<string, string>(capacity, comparer);

        }
        public NameValueCollection(INameValueCollection items) :
            this() {

            this.AddRange(items);

        }
        public NameValueCollection(IDictionary<string, string> items) :
            this() {

            foreach (var item in items)
                Add(item.Key, item.Value);

        }
        public NameValueCollection(IEnumerable<KeyValuePair<string, string>> items) {

            foreach (var item in items)
                Add(item.Key, item.Value);

        }

        public void Add(string key, string value) {

            underlyingDict.Add(EscapeKey(key), value);

        }
        public void Add(KeyValuePair<string, string> item) {

            underlyingDict.Add(EscapeKey(item.Key), item.Value);

        }

        public string Get(string key) {

            if (underlyingDict.TryGetValue(EscapeKey(key), out IReadOnlyCollection<string> items))
                return ItemsToString(items);

            return null;

        }
        public bool TryGetValue(string key, out string value) {

            return underlyingDict.TryGetValue(EscapeKey(key), out value);

        }
        public IEnumerable<string> GetValues(string key) {

            if (underlyingDict.TryGetValue(EscapeKey(key), out IReadOnlyCollection<string> values))
                return values;

            return Enumerable.Empty<string>();

        }
        public bool TryGetValues(string key, out IEnumerable<string> values) {

            if (underlyingDict.TryGetValue(EscapeKey(key), out IReadOnlyCollection<string> items)) {

                values = items;

                return true;

            }
            else {

                values = Enumerable.Empty<string>();

                return false;

            }

        }
        public void Set(string key, string value) {

            if (IsReadOnly)
                throw new NotSupportedException(ExceptionMessages.CollectionIsReadOnly);

            underlyingDict.Remove(EscapeKey(key));
            underlyingDict.Add(EscapeKey(key), value);

        }

        public bool ContainsKey(string key) {

            return underlyingDict.ContainsKey(EscapeKey(key));

        }

        public bool Contains(KeyValuePair<string, string> item) {

            return underlyingDict.Contains(EscapeKey(item.Key), item.Value);

        }
        public bool Remove(string key) {

            return underlyingDict.Remove(EscapeKey(key));

        }
        public bool Remove(KeyValuePair<string, string> item) {

            return underlyingDict.Remove(EscapeKey(item.Key), item.Value);

        }
        public void Clear() {

            underlyingDict.Clear();

        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {

            underlyingDict.CopyTo(array, arrayIndex);

        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {

            return underlyingDict.Keys
                .SelectMany(key => underlyingDict[key].Select(value => new KeyValuePair<string, string>(key, value)))
                .GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private const string ItemDelimiter = ", ";

        private readonly MultiValueDictionary<string, string> underlyingDict;

        private static string ItemsToString(IEnumerable<string> items) {

            return string.Join(ItemDelimiter, items);

        }
        private static string EscapeKey(string key) {

            return string.IsNullOrEmpty(key) ?
                "" :
                key;

        }

    }

}