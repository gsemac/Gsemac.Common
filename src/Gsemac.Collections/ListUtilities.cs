using Gsemac.Collections.Extensions;
using Gsemac.Collections.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public static class ListUtilities {

        // Public members

        public static void Move<T>(IList<T> items, int oldIndex, int newIndex) {

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (oldIndex < 0 || oldIndex > items.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(oldIndex), ExceptionMessages.IndexOutOfRange);

            if (newIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(newIndex), ExceptionMessages.IndexOutOfRange);

            // Remove the old item.

            T item = items[oldIndex];

            items.RemoveAt(oldIndex);

            // Insert the new item.

            items.Insert(newIndex, item);


        }
        public static void RemoveAll<T>(IList<T> items, int[] indicesToRemove) {

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (indicesToRemove is object) {

                // Items are removed by index in descending order so that their positions remain constant.
                // https://stackoverflow.com/a/9908607

                foreach (int index in indicesToRemove.OrderByDescending(i => i)) {

                    if (index < 0 || index > items.Count)
                        throw new IndexOutOfRangeException(ExceptionMessages.IndexOutOfRange);

                    items.RemoveAt(index);

                }

            }

        }
        public static void RemoveAll<T>(IList<T> items, IEnumerable<T> itemsToRemove) {

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (itemsToRemove is object && itemsToRemove.Any()) {

                foreach (T item in itemsToRemove)
                    items.Remove(item);

            }

        }
        public static void RemoveDuplicates<T>(IList<T> items) {

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            HashSet<T> itemsLookup = new HashSet<T>();
            List<T> distinctItems = new List<T>(items.Count);

            foreach (T item in items) {

                if (!itemsLookup.Contains(item)) {

                    itemsLookup.Add(item);
                    distinctItems.Add(item);

                }

            }

            items.Clear();

            items.AddRange(distinctItems);

        }

    }

}