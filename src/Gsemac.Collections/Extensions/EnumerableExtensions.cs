using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections.Extensions {

    public static class EnumerableExtensions {

        // Public members

        public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int length) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.Skip(Math.Max(0, source.Count() - length));

        }
        public static IEnumerable<TSource> PadLeft<TSource>(this IEnumerable<TSource> source, int length) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            int diff = length - source.Count();

            for (int i = 0; i < diff; ++i)
                yield return default;

            foreach (TSource value in source)
                yield return value;

        }
        public static IEnumerable<TSource> PadRight<TSource>(this IEnumerable<TSource> source, int length) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            int diff = length - source.Count();

            foreach (TSource value in source)
                yield return value;

            for (int i = 0; i < diff; ++i)
                yield return default;

        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource value) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            int index = 0;

            foreach (TSource element in source) {

                if (element.Equals(value))
                    return index;

                ++index;

            }

            // If we reach this point, the desired value was not found.

            return -1;

        }
        public static int IndexOf(this IEnumerable<string> source, string value, StringComparison comparisonType) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            int index = 0;

            foreach (string element in source) {

                if (element.Equals(value, comparisonType))
                    return index;

                ++index;

            }

            // If we reach this point, the desired value was not found.

            return -1;

        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return source.GroupBy(selector).Select(item => item.First());

        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IEqualityComparer<TKey> comparer) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            return source.GroupBy(selector, comparer).Select(item => item.First());

        }

        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.OrderBy((item) => random.Next());

        }
        public static TSource Random<TSource>(this IEnumerable<TSource> source) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.Shuffle().First();

        }
        public static TSource RandomOrDefault<TSource>(this IEnumerable<TSource> source) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.Shuffle().FirstOrDefault();

        }

        public static bool IsSorted<TSource>(this IEnumerable<TSource> source) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return IsSorted(source, Comparer<TSource>.Default);

        }
        public static bool IsSorted<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Zip(source.Skip(1), (curr, next) => comparer.Compare(curr, next) <= 0)
                .All(isSorted => isSorted);

        }

        public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.DefaultIfEmpty().Min();

        }
        public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.DefaultIfEmpty(defaultValue).Min();

        }
        public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return source.DefaultIfEmpty().Min(selector);

        }
        public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).MinOrDefault(defaultValue);

        }
        public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.DefaultIfEmpty().Max();

        }
        public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return source.DefaultIfEmpty(defaultValue).Max();

        }
        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return source.DefaultIfEmpty().Max(selector);

        }
        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).MaxOrDefault(defaultValue);

        }

        // Private members

        private static readonly Random random = new Random();

    }

}