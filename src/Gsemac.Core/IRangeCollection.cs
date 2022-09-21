using System;
using System.Collections.Generic;

namespace Gsemac.Core {

    public interface IRangeCollection<T> :
        ICollection<IRange<T>> where T :
        struct, IComparable, IFormattable {

        bool Contains(T value);
        bool IntersectsWith(IRange<T> range);

        void Sort();
        void Sort(IComparer<IRange<T>> comparer);

        void Condense();

    }

}