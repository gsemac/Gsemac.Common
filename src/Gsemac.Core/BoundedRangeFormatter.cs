using System;
using System.Globalization;
using System.Text;

namespace Gsemac.Core {

    public class BoundedRangeFormatter :
        RangeFormatterBase {

        // Public members

        public BoundedRangeFormatter() { }
        public BoundedRangeFormatter(IRangeFormattingOptions defaultOptions) :
            base(defaultOptions) {
        }

        public override string Format<T>(IRange<T> range, IRangeFormattingOptions options) {

            if (range is null)
                throw new ArgumentNullException(nameof(range));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            T start = options.Normalize ? range.Minimum : range.Start;
            T end = options.Normalize ? range.Maximum : range.End;

            StringBuilder sb = new StringBuilder();

            if (range.Contains(start))
                sb.Append("[");
            else
                sb.Append("(");

            sb.Append(start.ToString(null, CultureInfo.InvariantCulture));

            sb.Append(",");

            sb.Append(end.ToString(null, CultureInfo.InvariantCulture));

            if (range.Contains(end))
                sb.Append("]");
            else
                sb.Append(")");

            return sb.ToString();

        }

    }

}