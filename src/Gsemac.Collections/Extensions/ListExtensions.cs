using System.Collections.Generic;

namespace Gsemac.Collections.Extensions {

    public static class ListExtensions {

        // Public members

        public static void Move<T>(IList<T> items, int oldIndex, int newIndex) {

            ListUtilities.Move(items, oldIndex, newIndex);

        }
        public static void RemoveAll<T>(this IList<T> items, int[] indicesToRemove) {

            ListUtilities.RemoveAll(items, indicesToRemove);

        }
        public static void RemoveAll<T>(this IList<T> items, IEnumerable<T> itemsToRemove) {

            ListUtilities.RemoveAll(items, itemsToRemove);

        }

    }


}