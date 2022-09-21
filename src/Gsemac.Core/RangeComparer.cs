using System;
using System.Collections.Generic;

namespace Gsemac.Core {

    internal class RangeComparer<T> :
        IComparer<IRange<T>> where T :
        struct, IComparable, IFormattable {

        // Public members

        public static RangeComparer<T> Default => new RangeComparer<T>();

        public int Compare(IRange<T> x, IRange<T> y) {

            if (x is null)
                throw new ArgumentNullException(nameof(x));

            if (y is null)
                throw new ArgumentNullException(nameof(y));

            T xMin = x.Minimum;
            T yMin = y.Minimum;
            T xMax = x.Maximum;
            T yMax = y.Maximum;

            // If one range starts before the other, place the earlier range first.

            if (xMin.CompareTo(yMin) != 0)
                return xMin.CompareTo(yMin);

            // If the ranges start at the same point, place the one that ends earlier first.

            if (xMax.CompareTo(yMax) != 0)
                return xMax.CompareTo(yMax);

            // The ranges are equal.

            return 0;
        }

    }

}