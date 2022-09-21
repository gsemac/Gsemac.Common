using System;

namespace Gsemac.Core {

    public interface IRangeFormatter {

        string Format<T>(IRange<T> range) where T : struct, IComparable, IFormattable;
        string Format<T>(IRange<T> range, IRangeFormattingOptions options) where T : struct, IComparable, IFormattable;

    }

}