using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections.Extensions {

    public static class EnumerableExtensions {

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int length) {

            return source.Skip(Math.Max(0, source.Count() - length));

        }
        public static int IndexOf<T>(this IEnumerable<T> source, T value) {

            int index = 0;

            foreach (T element in source) {

                if (element.Equals(value))
                    return index;

                ++index;

            }

            // If we reach this point, the desired value was not found.

            return -1;

        }

    }

}