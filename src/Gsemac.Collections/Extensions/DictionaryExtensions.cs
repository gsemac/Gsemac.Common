using System.Collections.Generic;

namespace Gsemac.Collections.Extensions {

    public static class DictionaryExtensions {

        // Public members

        public static IDictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) {

            return DictionaryUtilities.Reverse(dictionary);

        }

    }

}