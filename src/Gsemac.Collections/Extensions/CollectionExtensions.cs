using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections.Extensions {

    public static class CollectionExtensions {

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) {

            if (collection is List<T> list) {

                list.AddRange(items);

            }
            else {

                foreach (T item in items)
                    collection.Add(item);

            }

        }
        public static void Insert<T>(this ICollection<T> collection, int index, T item) {

            if (collection is IList<T> list) {

                list.Insert(index, item);

            }
            else {

                collection.InsertRange(index, new[] { item });

            }

        }
        public static void InsertRange<T>(this ICollection<T> collection, int index, IEnumerable<T> items) {

            if (index < 0 || index > collection.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range. Must be non-negative and less than the size of the collection.");

            if (collection is List<T> list) {

                list.InsertRange(index, items);

            }
            else {

                // I know this is bad

                List<T> temp = new List<T>(collection);

                collection.Clear();

                collection.AddRange(temp.Take(index));
                collection.AddRange(items);
                collection.AddRange(temp.Skip(index));

            }

        }

    }

}