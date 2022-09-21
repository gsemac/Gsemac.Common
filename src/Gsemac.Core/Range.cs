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

            return this.ToString(RangeFormatter.Bounded);

        }

        public static IRange<T> Parse(string value) {

            if (!TryParse(value, out IRange<T> result))
                throw new FormatException(string.Format(ExceptionMessages.InvalidRangeString, value));

            return result;

        }
        public static bool TryParse(string value, out IRange<T> result) {

            result = null;

            if (value is null)
                return false;

            // Accept ranges in the form "[1,5)" or "1-4".

            string[] parts = value.Split(new[] { '-', ',' })
                .Select(part => part.Trim())
                .ToArray();

            // Our ranges must have at least 1 value, and no more than two (start and end).

            if (parts.Length <= 0 || parts.Length > 2)
                return false;

            string startStr = parts[0];
            string endStr = parts.Length > 1 ? parts[1] : parts[0];

            // Determine whether the bounds are inclusive or exclusive.

            bool startInclusive = !startStr.StartsWith("(");
            bool endInclusive = !endStr.EndsWith(")");

            startStr = startStr.TrimStart('(', '[');
            endStr = endStr.TrimEnd(')', ']');

            // Parse the bounds values.
            // Currently, the values are parsed as doubles before being converted to the desired type.

            if (!double.TryParse(startStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double startDouble) ||
                !double.TryParse(endStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double endDouble)) {

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

    }

}