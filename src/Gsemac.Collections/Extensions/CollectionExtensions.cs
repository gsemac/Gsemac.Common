using System.Collections.Generic;

namespace Gsemac.Collections.Extensions {

    public static class CollectionExtensions {

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) {

            if (collection is IList<T> list) {

                list.AddRange(items);

            }
            else {

                foreach (T item in items)
                    collection.Add(item);

            }

        }

    }

}
