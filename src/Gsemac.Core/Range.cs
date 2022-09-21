using Gsemac.Core.Properties;
using System;
using System.Globalization;
using System.Linq;

namespace Gsemac.Core {

    public class Range<T> :
        IRange<T> where T :
        struct, IComparable, IFormattable {

        // Public members

        public T Start { get; }
        public T End { get; }
        public bool IsAscending => Start.CompareTo(End) < 0;
        public bool IsDescending => Start.CompareTo(End) > 0;
        public T Minimum => Start.CompareTo(End) < 0 ? Start : End;
        public T Maximum => Start.CompareTo(End) > 0 ? Start : End;

        public Range(T value) :
            this(value, value) {
        }
        public Range(T start, T end) :
            this(start, true, end, true) {
        }
        public Range(T start, bool startInclusive, T end, bool endInclusive) {

            Start = start;
            End = end;
            this.startInclusive = startInclusive;
            this.endInclusive = endInclusive;

        }

        public bool Contains(T value) {

            if (startInclusive && value.CompareTo(Start) == 0)
                return true;

            if (endInclusive && value.CompareTo(End) == 0)
                return true;

            return value.CompareTo(Minimum) > 0 &&
                value.CompareTo(Maximum) < 0;

        }
        public bool Contains(IRange<T> range) {

            if (range is null)
                throw new ArgumentNullException(nameof(range));

            return Contains(range.Minimum) &&
                Contains(range.Maximum);

        }
        public bool IntersectsWith(IRange<T> range) {

            if (range is null)
                throw new ArgumentNullException(nameof(range));

            return Contains(range) ||
                range.Contains(this) ||
                Contains(range.Minimum) ||
                Contains(range.Maximum);

        }

        public int CompareTo(IRange<T> other) {

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            return new RangeComparer<T>().Compare(this, other);

        }

        public override bool Equals(object obj) {

            if (!(obj is IRange<T> other))
                return false;

            return other.GetHashCode().Equals(GetHashCode());

        }
        public override int GetHashCode() {

            return new HashCodeBuilder()
                .WithValue(Start)
                .WithValue(End)
                .WithValue(startInclusive)
                .WithValue(endInclusive)
                .Build();

        }
        public override string ToString() {

            return this.ToString(RangeFormatter.Default);

        }

        public static Range<T> Parse(string value) {

            return Parse(value, RangeParsingOptions.Default);

        }
        public static Range<T> Parse(string value, IRangeParsingOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (!TryParse(value, options, out Range<T> result))
                throw new FormatException(string.Format(ExceptionMessages.InvalidRangeString, value));

            return result;

        }
        public static bool TryParse(string value, out Range<T> result) {

            return TryParse(value, RangeParsingOptions.Default, out result);

        }
        public static bool TryParse(string value, IRangeParsingOptions options, out Range<T> result) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            result = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Accept ranges in the form "[1,5)" or "1-4".
            // Make sure to account for negative numbers (e.g. "-1--4").

            value = value.Trim();

            string[] parts = null;
            bool isBoundedRange = false;

            if (options.AllowBoundedRanges && (value.StartsWith("(") || value.StartsWith("["))) {

                // Parse a bounded range (e.g. "[1,5)".

                parts = value.Split(new[] { ',' }, 2)
                    .Select(part => part.Trim())
                    .ToArray();

                isBoundedRange = true;

            }
            else if (options.AllowDashedRanges) {

                // Parse a dashed range (e.g. "1-4").
                // Ignore leading dashes, as they can only be a negative sign.

                parts = value.TrimStart('-').Split(new[] { '-' }, 2)
                   .Select(part => part.Trim())
                   .ToArray();

                if (value.StartsWith("-"))
                    parts[0] = "-" + parts[0];

            }
            else {

                parts = new[] { value };

            }

            // Our ranges must have at least 1 value, and no more than two (start and end).

            if (parts is object && (parts.Length <= 0 || parts.Length > 2))
                return false;

            string startStr = parts[0];
            string endStr = parts.Length > 1 ? parts[1] : startStr;

            // Determine whether the bounds are inclusive or exclusive.

            bool startInclusive = true;
            bool endInclusive = true;

            if (options.AllowBoundedRanges && isBoundedRange) {

                startInclusive = !startStr.StartsWith("(");
                endInclusive = !endStr.EndsWith(")");

                startStr = startStr.TrimStart('(', '[');
                endStr = endStr.TrimEnd(')', ']');

            }

            // Parse the bounds values.
            // Currently, the values are parsed as doubles before being converted to the desired type.

            if (!double.TryParse(startStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double startDouble) ||
                !double.TryParse(endStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double endDouble)) {

                return false;

            }
            else if (!options.AllowNegativeNumbers && (startDouble < 0 || endDouble < 0)) {

                return false;

            }

            try {

                T start = (T)Convert.ChangeType(startDouble, typeof(T), CultureInfo.InvariantCulture);
                T end = (T)Convert.ChangeType(endDouble, typeof(T), CultureInfo.InvariantCulture);

                result = new Range<T>(start, startInclusive, end, endInclusive);

            }
            catch (Exception) {

                return false;

            }

            return true;

        }

        // Private members

        private readonly bool startInclusive;
        private readonly bool endInclusive;

    }

    public static class Range {

        // Public members

        public static IRange<T> Create<T>(T value) where T : struct, IComparable, IFormattable {

            return new Range<T>(value);

        }
        public static IRange<T> Create<T>(T start, T end) where T : struct, IComparable, IFormattable {

            return new Range<T>(start, end);

        }
        public static IRange<T> Create<T>(T start, bool startInclusive, T end, bool endInclusive) where T : struct, IComparable, IFormattable {

            return new Range<T>(start, startInclusive, end, endInclusive);

        }

        public static IRange<T> Combine<T>(IRange<T> range1, IRange<T> range2) where T : struct, IComparable, IFormattable {

            if (range1 is null)
                throw new ArgumentNullException(nameof(range1));

            if (range2 is null)
                throw new ArgumentNullException(nameof(range2));

            T start = range1.Minimum.CompareTo(range2.Minimum) < 0 ? range1.Minimum : range2.Minimum;
            T end = range1.Maximum.CompareTo(range2.Maximum) > 0 ? range1.Maximum : range2.Maximum;

            bool startInclusive = range1.Contains(start) || range2.Contains(start);
            bool endInclusive = range1.Contains(end) || range2.Contains(end);

            return Create(start, startInclusive, end, endInclusive);

        }
        public static IRange<T> Subtract<T>(IRange<T> range1, IRange<T> range2) where T : struct, IComparable, IFormattable {

            if (range1 is null)
                throw new ArgumentNullException(nameof(range1));

            if (range2 is null)
                throw new ArgumentNullException(nameof(range2));

            // If the two ranges do not intersect, do nothing.

            if (!range2.IntersectsWith(range1))
                return range1;

            T start = range1.Minimum.CompareTo(range2.Minimum) < 0 ? range1.Minimum : range2.Maximum;
            T end = range1.Maximum.CompareTo(range2.Maximum) > 0 ? range1.Maximum : range2.Minimum;

            // For two equal ranges, we'll end up with start > end.
            // In this case, we want to produce an empty range.

            if (start.CompareTo(end) > 0)
                start = end;

            bool startInclusive = !range2.Contains(start);
            bool endInclusive = !range2.Contains(end);

            return Create(start, startInclusive, end, endInclusive);

        }

        public static IRange<T> Normalize<T>(IRange<T> range) where T : struct, IComparable, IFormattable {

            if (range is null)
                throw new ArgumentNullException(nameof(range));

            T start = range.Minimum;
            T end = range.Maximum;

            bool startInclusive = range.Contains(start);
            bool endInclusive = range.Contains(end);

            return Create(start, startInclusive, end, endInclusive);

        }

    }

}