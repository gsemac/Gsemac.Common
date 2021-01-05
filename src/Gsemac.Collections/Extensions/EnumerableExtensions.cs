using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections.Extensions {

    public static class EnumerableExtensions {

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int length) {

            return source.Skip(Math.Max(0, source.Count() - length));

        }
        public static IEnumerable<T> PadLeft<T>(this IEnumerable<T> source, int length) {

            int diff = length - source.Count();

            for (int i = 0; i < diff; ++i)
                yield return default;

            foreach (T value in source)
                yield return value;

        }
        public static IEnumerable<T> PadRight<T>(this IEnumerable<T> source, int length) {

            int diff = length - source.Count();

            foreach (T value in source)
                yield return value;

            for (int i = 0; i < diff; ++i)
                yield return default;

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