using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestExtensions {

        // Public members

        public static void SetHeader(this HttpWebRequest webRequest, string headerName, string headerValue) {

            switch (headerName.ToLowerInvariant()) {

                case "accept":
                    webRequest.Accept = headerValue;
                    break;

                case "content-type":
                    webRequest.ContentType = headerValue;
                    break;

                case "range": {

                        var range = ParseRangeHeader(headerValue);

                        webRequest.AddRange(range.Item1, range.Item2);

                    }
                    break;

                case "user-agent":
                    webRequest.UserAgent = headerValue;
                    break;

                default:
                    webRequest.Headers.Set(headerName, headerValue);
                    break;

            }

        }

        // Private members

        private static Tuple<long, long> ParseRangeHeader(string headerValue) {

            Match match = Regex.Match(headerValue, "");

            if (!match.Success)
                throw new ArgumentNullException(nameof(headerValue));

            long first = long.Parse(match.Groups[1].Value);
            long second = long.Parse(match.Groups[2].Value);

            return new Tuple<long, long>(first, second);

        }

    }

}