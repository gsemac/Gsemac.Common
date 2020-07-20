using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections.Extensions {

    public static class EnumerableExtensions {

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int length) {

            return source.Skip(Math.Max(0, source.Count() - length));

        }

    }

}