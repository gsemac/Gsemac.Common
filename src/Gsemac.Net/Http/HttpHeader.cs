using Gsemac.Net.Properties;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Http {

    public class HttpHeader :
        IHttpHeader {

        // Public members

        public string Name { get; }
        public string Value { get; }

        public HttpHeader(string name, string value) {

            if (name is null)
                throw new ArgumentNullException(nameof(name));

            Name = name;

            // An empty header value is valid according to the HTTP spec.

            Value = value ?? "";

        }
        public HttpHeader(HttpRequestHeader header, string value) :
            this(HttpUtilities.GetHeaderName(header), value) {
        }
        public HttpHeader(HttpResponseHeader header, string value) :
            this(HttpUtilities.GetHeaderName(header), value) {
        }

        public static HttpHeader Parse(string httpHeader) {

            if (TryParse(httpHeader, out HttpHeader result)) {

                return result;

            }
            else {

                throw new FormatException(string.Format(ExceptionMessages.InvalidHttpHeaderWithString, httpHeader));

            }

        }
        public static bool TryParse(string httpHeader, out HttpHeader result) {

            Match httpHeaderMatch = Regex.Match(httpHeader, @"^(.+?):\s(.+?)$");

            if (httpHeaderMatch.Success) {

                string name = httpHeaderMatch.Groups[1].Value;
                string value = httpHeaderMatch.Groups[2].Value;

                result = new HttpHeader(name, value);

            }
            else {

                result = null;

            }

            return result is object;

        }

        public override string ToString() {

            return $"{Name}: {Value}";

        }

    }

}