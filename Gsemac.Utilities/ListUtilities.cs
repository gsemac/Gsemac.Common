using System.Collections.Generic;

namespace Gsemac.Utilities {

    public static class ListUtilities {

        public static void RemoveDuplicates<T>(List<T> items) {

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