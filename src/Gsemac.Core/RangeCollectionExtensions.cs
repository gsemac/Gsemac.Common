using System;
using System.Linq;

namespace Gsemac.Core {

    public static class RangeCollectionExtensions {

        // Public members

        public static string ToString<T>(this IRangeCollection<T> collection, IRangeFormatter formatter) where T :
            struct, IComparable, IFormattable {

            return ToString(collection, formatter, RangeFormattingOptions.Default);

        }
        public static string ToString<T>(this IRangeCollection<T> collection, IRangeFormatter formatter, IRangeFormattingOptions options) where T :
            struct, IComparable, IFormattable {

            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            return string.Join(", ", collection.Select(item => item.ToString(formatter, options)));

        }

    }

}