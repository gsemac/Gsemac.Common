using Gsemac.Net.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Http.Headers {

    public sealed class AcceptEncodingHeaderValue {

        // Public members

        public DecompressionMethods EncodingMethods { get; set; } = DecompressionMethods.None;

        public AcceptEncodingHeaderValue(DecompressionMethods decompressionMethods) {

            EncodingMethods = decompressionMethods;

        }
        public AcceptEncodingHeaderValue(string value) {

            AcceptEncodingHeaderValue header = Parse(value);

            EncodingMethods = header.EncodingMethods;

        }

        public static AcceptEncodingHeaderValue Parse(string value) {

            if (TryParse(value, out AcceptEncodingHeaderValue result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.InvalidHttpAcceptEncodingHeaderWithString, value));

        }
        public static bool TryParse(string value, out AcceptEncodingHeaderValue result) {

            result = null;

            DecompressionMethods methods = DecompressionMethods.None;

            if (!string.IsNullOrWhiteSpace(value)) {

                foreach (string encodingName in value.Split(',').Select(v => v.Trim().ToLowerInvariant()).Where(v => !string.IsNullOrWhiteSpace(v))) {

                    switch (encodingName) {

                        case "gzip":

                            methods |= DecompressionMethods.GZip;

                            break;

                        case "deflate":

                            methods |= DecompressionMethods.Deflate;

                            break;

                        case "br":

                            methods |= (DecompressionMethods)Polyfills.System.Net.DecompressionMethods.Brotli;

                            break;

                        default:

                            // Invalid encoding encountered.

                            return false;

                    }

                }

            }

            result = new AcceptEncodingHeaderValue(methods);

            return true;

        }

        public override string ToString() {

            List<string> methods = new List<string>();

            if (EncodingMethods.HasFlag(DecompressionMethods.GZip))
                methods.Add("gzip");

            if (EncodingMethods.HasFlag(DecompressionMethods.Deflate))
                methods.Add("deflate");

            if (EncodingMethods.HasFlag((DecompressionMethods)Polyfills.System.Net.DecompressionMethods.Brotli))
                methods.Add("br");

            return string.Join(", ", methods);

        }

    }

}