using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public static class MultiDictionaryExtensions {

        public static void Add<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, TKey key, TValue value) {

            ICollection<TValue> existingItems = dictionary[key];

            existingItems.Add(value);

        }
        public static void Add<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item) {

            dictionary.Add(item.Key, item.Value);

        }
        public static void AddRange<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, TKey key, IEnumerable<TValue> values) {

            ICollection<TValue> existingItems = dictionary[key];

            foreach (TValue item in values)
                existingItems.Add(item);

        }
        public static bool Contains<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item) {

            if (dictionary.TryGetValue(item.Key, out ICollection<TValue> values))
                return values.Contains(item.Value);

            return false;

        }
        public static void CopyTo<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue>[] array, int arrayIndex) {

            dictionary.Keys.SelectMany(key => dictionary[key].Select(value => new KeyValuePair<TKey, TValue>(key, value)))
                .ToArray()
                .CopyTo(array, arrayIndex);

        }
        public static bool Remove<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, TKey key, TValue value) {

            ICollection<TValue> items = dictionary[key];

            return items.Remove(value);

        }
        public static bool Remove<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> item) {

            return dictionary.Remove(item.Key, item.Value);

        }
        public static bool TryGetValue<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, TKey key, out TValue value) {

            if (dictionary.TryGetValue(key, out ICollection<TValue> values) && values.Any()) {

                value = values.First();

                return true;

            }

            value = default;

            return false;

        }

    }

}