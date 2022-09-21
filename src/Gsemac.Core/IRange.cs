using System;

namespace Gsemac.Core {

    public interface IRange<T> : 
        IComparable<IRange<T>> where T :
        struct, IComparable, IFormattable {

        T Start { get; }
        T End { get; }
        bool IsAscending { get; }
        bool IsDescending { get; }
        T Minimum { get; }
        T Maximum { get; }

        bool Contains(T value);
        bool Contains(IRange<T> range);
        bool IntersectsWith(IRange<T> range);

    }

}