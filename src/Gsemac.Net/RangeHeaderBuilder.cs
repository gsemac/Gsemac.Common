using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net {

    public class RangeHeaderBuilder :
        IRangeHeaderBuilder {

        // Public members

        public RangeHeaderBuilder() { }
        public RangeHeaderBuilder(string range) {

            // Read the range specifier from the given string. An exception is thrown if a range specifier cannot be found.

            rangeSpecifier = GetRangeSpecifier(range);

            if (string.IsNullOrWhiteSpace(rangeSpecifier) && !string.IsNullOrWhiteSpace(range))
                throw new ArgumentException("Missing range specifier.", nameof(range));

            if (!string.IsNullOrEmpty(range))
                rangeBuilder.Append(range.Trim());

        }

        public IRangeHeaderBuilder AddRange(int from, int to) => AddRange((long)from, to);
        public IRangeHeaderBuilder AddRange(long from, long to) => AddRange("bytes", from, to);
        public IRangeHeaderBuilder AddRange(int range) => AddRange((long)range);
        public IRangeHeaderBuilder AddRange(long range) => AddRange("bytes", range);
        public IRangeHeaderBuilder AddRange(string rangeSpecifier, long from, long to) {

            if (string.IsNullOrWhiteSpace(this.rangeSpecifier))
                this.rangeSpecifier = rangeSpecifier;
            else if (!rangeSpecifier.Equals(this.rangeSpecifier))
                throw new ArgumentException("Cannot set multiple range specifiers.", nameof(rangeSpecifier));

            if (rangeBuilder.Length <= 0)
                rangeBuilder.Append($"{rangeSpecifier}=");
            else
                rangeBuilder.Append(", ");

            if (from <= to)
                rangeBuilder.Append($"{from.ToString(CultureInfo.InvariantCulture)}-{to.ToString(CultureInfo.InvariantCulture)}");
            else
                rangeBuilder.Append($"{from.ToString(CultureInfo.InvariantCulture)}-");

            return this;

        }
        public IRangeHeaderBuilder AddRange(string rangeSpecifier, int range) => AddRange(rangeSpecifier, (long)range);
        public IRangeHeaderBuilder AddRange(string rangeSpecifier, long range) {

            if (range >= 0)
                AddRange(rangeSpecifier, range, 0);
            else
                AddRange(rangeSpecifier, 0, Math.Abs(range));

            return this;

        }
        public IRangeHeaderBuilder AddRange(string rangeSpecifier, int from, int to) => AddRange(rangeSpecifier, (long)from, to);

        public override string ToString() {

            return rangeBuilder.ToString();

        }

        // Private members

        private string rangeSpecifier;
        private readonly StringBuilder rangeBuilder = new StringBuilder();

        private string GetRangeSpecifier(string range) {

            Match rangeSpecifierMatch = Regex.Match(@"^([^=]+?)=", range);

            if (rangeSpecifierMatch.Success)
                return rangeSpecifierMatch.Groups[1].Value;

            return string.Empty;

        }

    }

}