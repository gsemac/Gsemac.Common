using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.JavaScript.Extensions {

    public static class JsArrayExtensions {

        public static T Pop<T>(this IList<T> array) {

            T last = array.LastOrDefault();

            if (array.Count > 0)
                array.RemoveAt(array.Count - 1);

            return last;

        }
        public static T[] Reverse<T>(this T[] array) {

            if (array is null)
                return null;

            Array.Reverse(array);

            return array;

        }
        public static IList<T> Reverse<T>(this IList<T> array) {

            if (array is null)
                return null;

            if (array is List<T> list) {

                list.Reverse();

            }
            else {

                T[] items = array.Reverse().ToArray();

                array.Clear();

                foreach (T item in items)
                    array.Add(item);

            }

            return array;

        }
        public static T Shift<T>(this IList<T> array) {

            T first = array.FirstOrDefault();

            if (array.Count > 0)
                array.RemoveAt(0);

            return first;

        }
        public static IList<T> Splice<T>(this IList<T> array, int start) {

            return Splice(array, start, array.Count() - start);

        }
        public static IList<T> Splice<T>(this IList<T> array, int start, int deleteCount) {

            if (array is null)
                return null;

            if (deleteCount < 0)
                deleteCount = 0;
            else if (deleteCount > array.Count() - start)
                deleteCount = array.Count() - start;

            IList<T> removedItems = array.Skip(start).Take(deleteCount).ToList();

            if (array is List<T> list) {

                list.RemoveRange(start, deleteCount);

            }
            else {

                for (int i = start + deleteCount; i > start; --i)
                    removedItems.RemoveAt(i);

            }

            return removedItems;

        }

    }

}