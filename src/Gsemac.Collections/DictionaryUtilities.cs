using System;
using System.Collections.Generic;

namespace Gsemac.Collections {

    public static class DictionaryUtilities {

        // Public members

        public static IDictionary<TValue, TKey> Reverse<TKey, TValue>(IDictionary<TKey, TValue> dictionary) {

            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));

            IDictionary<TValue, TKey> result = new Dictionary<TValue, TKey>();

            // If the dictionary contains duplicate keys (values), only the latest value will be mapped to the key.

            foreach (var pair in dictionary)
                result[pair.Value] = pair.Key;

            return result;

        }

    }

}