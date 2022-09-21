using System;

namespace Gsemac.Core {

    public static class RangeExtensions {

        // Public members

        public static string ToString<T>(this IRange<T> range, IRangeFormatter formatter) where T :
           struct, IComparable, IFormattable {

            return ToString(range, formatter, RangeFormattingOptions.Default);

        }
        public static string ToString<T>(this IRange<T> range, IRangeFormatter formatter, IRangeFormattingOptions options) where T :
            struct, IComparable, IFormattable {

            if (range is null)
                throw new ArgumentNullException(nameof(range));

            if (formatter is null)
                formatter = RangeFormatter.Default;

            return formatter.Format(range, options);

        }

    }

}