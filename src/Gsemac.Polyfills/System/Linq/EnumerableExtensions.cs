using System;
using System.Collections.Generic;

namespace Gsemac.Polyfills.System.Linq {

    public static class EnumerableExtensions {

        // Public members

        /// <summary>
        /// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to chunk.</param>
        /// <param name="size">Maximum size of each chunk.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1.</exception>
        public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int size) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size));

            // This could be improved by using an array buffer instead of a list, which works for all but the last chunk.

            List<TSource> chunk = new List<TSource>(size);

            foreach (TSource item in source) {

                chunk.Add(item);

                if (chunk.Count == size) {

                    yield return chunk.ToArray();

                    chunk = new List<TSource>(size);

                }

            }

            if (chunk.Count > 0)
                yield return chunk.ToArray();

        } // .NET 6 and later

    }

}