using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public static class NameValueCollectionExtensions {

        public static void Add(this INameValueCollection nameValueCollection, INameValueCollection items) {

            foreach (KeyValuePair<string, string> item in items)
                nameValueCollection.Add(item);

        }
        public static string Get(this INameValueCollection nameValueCollection, string key) {

            return nameValueCollection[key];

        }
        public static void Set(this INameValueCollection nameValueCollection, string key, string value) {

            nameValueCollection[key] = value;

        }

        public static bool HasKeys(this INameValueCollection nameValueCollection) {

            return nameValueCollection.Keys.Any(key => !(key is null));

        }

    }

}