using Gsemac.Net.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Http.Headers {

    public sealed class AcceptEncodingHeaderValue {

        // Public members

        public ICollection<ContentEncoding> ContentEncodings { get; } = new List<ContentEncoding>();
        public DecompressionMethods DecompressionMethods => GetDecompressionMethods();

        public AcceptEncodingHeaderValue(DecompressionMethods decompressionMethods) {

            foreach (string encodingName in GetEncodingNames(decompressionMethods))
                ContentEncodings.Add(new ContentEncoding(encodingName));

        }
        public AcceptEncodingHeaderValue(string value) {

            AcceptEncodingHeaderValue parsedValue = Parse(value);

            ContentEncodings = parsedValue.ContentEncodings;

        }

        public static AcceptEncodingHeaderValue Parse(string value) {

            if (TryParse(value, out AcceptEncodingHeaderValue result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.InvalidHttpAcceptEncodingHeaderWithString, value));

        }
        public static bool TryParse(string value, out AcceptEncodingHeaderValue result) {

            result = new AcceptEncodingHeaderValue();

            if (!string.IsNullOrWhiteSpace(value)) {

                foreach (string encodingStr in value.Split(',').Where(v => !string.IsNullOrWhiteSpace(v))) {

                    if (!ContentEncoding.TryParse(encodingStr, out ContentEncoding parsedContentEncoding))
                        return false;

                    result.ContentEncodings.Add(parsedContentEncoding);

                }

            }

            return true;

        }

        public override string ToString() {

            return string.Join(", ", ContentEncodings.OrderBy(e => e, new ContentEncodingComparer()));

        }

        // Private members

        private AcceptEncodingHeaderValue() { }

        private DecompressionMethods GetDecompressionMethods() {

            DecompressionMethods result = DecompressionMethods.None;

            foreach (ContentEncoding contentEncoding in ContentEncodings)
                result |= contentEncoding.DecompressionMethod;

            return result;

        }

        private static IEnumerable<string> GetEncodingNames(DecompressionMethods decompressionMethods) {

            List<string> encodingNames = new List<string>();

            if (decompressionMethods.HasFlag(DecompressionMethods.GZip))
                encodingNames.Add("gzip");

            if (decompressionMethods.HasFlag(DecompressionMethods.Deflate))
                encodingNames.Add("deflate");

            if (decompressionMethods.HasFlag((DecompressionMethods)Polyfills.System.Net.DecompressionMethods.Brotli))
                encodingNames.Add("br");

            if (decompressionMethods.HasFlag((DecompressionMethods)Polyfills.System.Net.DecompressionMethods.All))
                encodingNames.Add("*");

            return encodingNames;

        }

    }

}