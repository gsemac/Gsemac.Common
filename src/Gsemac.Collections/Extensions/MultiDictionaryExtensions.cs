using System.Collections.Generic;

namespace Gsemac.Collections.Extensions {

    public static class MultiDictionaryExtensions {

        public static void AddRange<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, TKey key, IEnumerable<TValue> items) {

            ICollection<TValue> existingItems = dictionary[key];

            foreach (TValue item in items)
                existingItems.Add(item);

        }
        public static bool RemoveItem<TKey, TValue>(this MultiDictionary<TKey, TValue> dictionary, TKey key, TValue item) {

            ICollection<TValue> items = dictionary[key];

            return items.Remove(item);

        }

    }

}