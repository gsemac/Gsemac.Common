using Gsemac.Net.Properties;
using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Http.Headers {

    public sealed class ContentEncoding {

        // Public members

        public string Name { get; }
        public double QualityValue { get; }
        public DecompressionMethods DecompressionMethod { get; }

        public ContentEncoding(string value) {

            ContentEncoding result = Parse(value);

            Name = result.Name;
            QualityValue = result.QualityValue;
            DecompressionMethod = result.DecompressionMethod;

        }
        public ContentEncoding(DecompressionMethods decompressionMethod) :
            this(GetEncodingName(decompressionMethod), 0, decompressionMethod) {
        }

        public override string ToString() {

            if (QualityValue <= 0)
                return Name;

            // Note that up to 3 decimal places are actually allowed for quality values.
            // https://developer.mozilla.org/en-US/docs/Glossary/Quality_values

            string qualityValueStr = QualityValue.ToString("N1", CultureInfo.InvariantCulture);

            if (qualityValueStr == "0.0")
                return Name;

            return $"{Name};q={qualityValueStr}";

        }

        public static ContentEncoding Parse(string value) {

            if (TryParse(value, out ContentEncoding result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.InvalidContentEncodingWithString, value));

        }
        public static bool TryParse(string value, out ContentEncoding result) {

            result = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.Trim().ToLowerInvariant();
            DecompressionMethods methods = DecompressionMethods.None;

            Match m = Regex.Match(value, @"^(?<encoding>identity|gzip|compress|deflate|br|zstd|\*)(?:;q=(?<qvalue>\d+(?:\.\d+))?)?$", RegexOptions.IgnoreCase);

            if (!m.Success)
                return false;

            string name = m.Groups["encoding"].Value;
            string qValueStr = m.Groups["qvalue"].Value;
            double qValue = 0;

            if (!string.IsNullOrWhiteSpace(qValueStr) && !double.TryParse(qValueStr, NumberStyles.Float, CultureInfo.InvariantCulture, out qValue))
                return false;

            switch (name) {

                case "*":
                    methods = (DecompressionMethods)Polyfills.System.Net.DecompressionMethods.All;
                    break;

                case "gzip":

                    methods |= DecompressionMethods.GZip;

                    break;

                case "deflate":

                    methods |= DecompressionMethods.Deflate;

                    break;

                case "br":

                    methods |= (DecompressionMethods)Polyfills.System.Net.DecompressionMethods.Brotli;

                    break;

            }

            result = new ContentEncoding(name.ToLowerInvariant(), qValue, methods);

            return true;

        }

        // Private members

        private ContentEncoding(string name, double qualityValue, DecompressionMethods decompressionMethod) {

            Name = name;
            QualityValue = qualityValue;
            DecompressionMethod = decompressionMethod;

        }

        private static string GetEncodingName(DecompressionMethods decompressionMethod) {

            if (decompressionMethod.HasFlag(DecompressionMethods.GZip))
                return "gzip";

            if (decompressionMethod.HasFlag(DecompressionMethods.Deflate))
                return "deflate";

            if (decompressionMethod.HasFlag((DecompressionMethods)Polyfills.System.Net.DecompressionMethods.Brotli))
                return "br";

            if (decompressionMethod.HasFlag((DecompressionMethods)Polyfills.System.Net.DecompressionMethods.All))
                return "*";

            return string.Empty;

        }

    }

}