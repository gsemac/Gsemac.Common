using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public static class MultiDictionaryExtensions {

        public static void Add<TKey, TValue>(this MultiValueDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item) {

            dictionary.Add(item.Key, item.Value);

        }
        public static bool Contains<TKey, TValue>(this MultiValueDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item) {

            return dictionary.Contains(item.Key, item.Value);

        }
        public static void CopyTo<TKey, TValue>(this MultiValueDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue>[] array, int arrayIndex) {

            dictionary.Keys.SelectMany(key => dictionary[key].Select(value => new KeyValuePair<TKey, TValue>(key, value)))
                .ToArray()
                .CopyTo(array, arrayIndex);

        }
        public static bool Remove<TKey, TValue>(this MultiValueDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item) {

            return dictionary.Remove(item.Key, item.Value);

        }
        public static bool TryGetValue<TKey, TValue>(this MultiValueDictionary<TKey, TValue> dictionary, TKey key, out TValue value) {

            if (dictionary.TryGetValue(key, out IReadOnlyCollection<TValue> values) && values.Any()) {

                value = values.First();

                return true;

            }

            value = default;

            return false;

        }

    }

}