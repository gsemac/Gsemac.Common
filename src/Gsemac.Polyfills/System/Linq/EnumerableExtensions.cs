using System;
using System.Collections.Generic;

namespace Gsemac.Polyfills.System.Linq {

    public static class EnumerableExtensions {

        // Public members

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