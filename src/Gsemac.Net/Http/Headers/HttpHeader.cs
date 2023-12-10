using Gsemac.Net.Properties;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Http.Headers {

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

            // Http headers are of the form "name: value".

            // Whitespace surrounding the field name is never trimmed, but whitspace surrounding the field value is always trimmed: https://stackoverflow.com/a/61632443/5383169
            // Headers can also have empty field values: https://stackoverflow.com/a/12131993/5383169

            Match httpHeaderMatch = Regex.Match(httpHeader, @"^(?<name>.+?):\s*(?<value>\S*?)\s*$");

            if (httpHeaderMatch.Success) {

                string name = httpHeaderMatch.Groups["name"].Value;
                string value = httpHeaderMatch.Groups["value"].Value;

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