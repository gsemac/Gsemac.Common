using Gsemac.Net.Properties;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Http.Headers {

    public sealed class RefreshHeaderValue {

        // Public members

        public TimeSpan Timeout { get; } = TimeSpan.Zero;
        public string Url { get; }

        public RefreshHeaderValue(string url, TimeSpan timeout) {

            Timeout = timeout;
            Url = url;

        }
        public RefreshHeaderValue(string url, int timeoutSeconds) :
            this(url, TimeSpan.FromSeconds(timeoutSeconds)) {
        }
        public RefreshHeaderValue(string value) {

            RefreshHeaderValue header = Parse(value);

            Timeout = header.Timeout;
            Url = header.Url;

        }

        public static RefreshHeaderValue Parse(string value) {

            if (TryParse(value, out RefreshHeaderValue result))
                return result;

            throw new FormatException(string.Format(ExceptionMessages.InvalidHttpRefreshHeaderWithString, value));

        }
        public static bool TryParse(string value, out RefreshHeaderValue result) {

            result = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Parse the parameters from the header value.
            // The value can take multiple forms with different labels and delimiters, such as:
            // 5
            // 5; url=https://www.example.com/
            // 3,https://www.example.com/

            Match httpHeaderMatch = Regex.Match(value.Trim(), @"^(?<timeout>\d+)\s*[;,]?(?:\s*(?:url=)?(?<url>.+?))?$", RegexOptions.IgnoreCase);

            if (httpHeaderMatch.Success) {

                string timeoutStr = httpHeaderMatch.Groups["timeout"].Value;
                string url = httpHeaderMatch.Groups["url"].Value;

                if (int.TryParse(timeoutStr, out int timeout))
                    result = new RefreshHeaderValue(url, Math.Max(0, timeout));

            }

            return result is object;

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(((int)Timeout.TotalSeconds).ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrWhiteSpace(Url))
                sb.Append($"; url={Url}");

            return sb.ToString();

        }

    }

}