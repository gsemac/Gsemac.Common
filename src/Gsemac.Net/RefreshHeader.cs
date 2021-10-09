using System;
using System.Text.RegularExpressions;

namespace Gsemac.Net {

    public class RefreshHeader :
        IHttpHeader {

        // Public members

        public string Name => RefreshHeaderName;
        public string Value => GetValue();

        public TimeSpan Timeout { get; } = TimeSpan.FromSeconds(0);
        public string Url { get; }

        public RefreshHeader(string url, TimeSpan timeout) {

            Timeout = timeout;
            Url = url;

        }
        public RefreshHeader(string url, int timeout) :
            this(url, TimeSpan.FromSeconds(timeout)) {
        }
        public RefreshHeader(string value) {

            RefreshHeader header = Parse($"{RefreshHeaderName}: {value}");

            Timeout = header.Timeout;
            Url = header.Url;

        }

        public static RefreshHeader Parse(string httpHeader) {

            if (TryParse(httpHeader, out RefreshHeader result))
                return result;
            else
                throw new ArgumentException(Properties.ExceptionMessages.InvalidHttpHeader, nameof(httpHeader));


        }
        public static bool TryParse(string httpHeader, out RefreshHeader result) {

            result = null;

            if (HttpHeader.TryParse(httpHeader, out HttpHeader parsedHttpHeader) && parsedHttpHeader.Name.Equals(RefreshHeaderName, StringComparison.OrdinalIgnoreCase)) {

                // Parse the parameters from the header value.
                // The refresh header can take multiple forms with different labels and delimiters, such as:
                // Refresh: 5; url=https://www.example.com/
                // Refresh: 3,https://www.example.com/

                Match httpHeaderMatch = Regex.Match(parsedHttpHeader.Value, @"^(?<timeout>\d+)\s*[;,]\s*(?:url=)?(?<url>.+?)$", RegexOptions.IgnoreCase);

                if (httpHeaderMatch.Success) {

                    string timeoutStr = httpHeaderMatch.Groups["timeout"].Value;
                    string url = httpHeaderMatch.Groups["url"].Value;

                    if (int.TryParse(timeoutStr, out int timeout))
                        result = new RefreshHeader(url, timeout);

                }

            }

            return !(result is null);

        }

        public override string ToString() {

            return $"{Name}: {GetValue()}";

        }

        // Private members

        private const string RefreshHeaderName = "refresh";

        private string GetValue() {

            return $"{(int)Timeout.TotalSeconds}; url={Url}";

        }

    }

}