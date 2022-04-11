using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections.Extensions {

    public static class EnumerableExtensions {

        // Public members

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

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) {

            return source.GroupBy(selector).Select(item => item.First());

        }
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, IEqualityComparer<TKey> comparer) {

            return source.GroupBy(selector, comparer).Select(item => item.First());

        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) {

            return source.OrderBy((item) => random.Next());

        }
        public static T Random<T>(this IEnumerable<T> source) {

            return source.Shuffle().First();

        }
        public static T RandomOrDefault<T>(this IEnumerable<T> source) {

            return source.Shuffle().FirstOrDefault();

        }

        public static bool IsSorted<T>(this IEnumerable<T> source) {

            return IsSorted(source, Comparer<T>.Default);

        }
        public static bool IsSorted<T>(this IEnumerable<T> source, IComparer<T> comparer) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Zip(source.Skip(1), (curr, next) => comparer.Compare(curr, next) <= 0)
                .All(isSorted => isSorted);

        }

        // Private members

        private static readonly Random random = new Random();

    }

}