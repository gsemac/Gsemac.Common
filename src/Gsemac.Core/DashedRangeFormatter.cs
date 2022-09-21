using System;
using System.Globalization;

namespace Gsemac.Core {

    public class DashedRangeFormatter :
        RangeFormatterBase {

        // Public members

        public DashedRangeFormatter() { }
        public DashedRangeFormatter(IRangeFormattingOptions defaultOptions) :
            base(defaultOptions) {
        }

        public override string Format<T>(IRange<T> range, IRangeFormattingOptions options) {

            if (range is null)
                throw new ArgumentNullException(nameof(range));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            T start = options.Normalize ? range.Minimum : range.Start;
            T end = options.Normalize ? range.Maximum : range.End;

            if (start.Equals(end))
                return start.ToString(null, CultureInfo.InvariantCulture);

            return $"{start.ToString(null, CultureInfo.InvariantCulture)}-{end.ToString(null, CultureInfo.InvariantCulture)}";

        }

    }

}