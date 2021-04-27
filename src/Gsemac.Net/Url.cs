using Gsemac.Collections.Extensions;
using Gsemac.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net {

    public sealed class Url :
        IUrl {

        // Public members

        public string Scheme {
            get => scheme;
            set => SetScheme(value);
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host => GetHost();
        public string Hostname { get; set; }
        public string DomainName => GetDomainName(ToString());
        public int? Port { get; set; }
        public string Root => GetRoot();
        public string Path { get; set; }
        public string Fragment { get; set; }
        public IDictionary<string, string> QueryParameters { get; }

        public Url(string url) {

            Match match = Regex.Match(url, @"^(?<scheme>.+?:)?(?:\/\/)?(?<credentials>.+?:.+?@)?(?<hostname>.+?)(?<path>\/.*?)?(?<query>\?.+?)?(?<fragment>#.+?)?$");

            if (!match.Success)
                throw new ArgumentException(Properties.ExceptionMessages.MalformedUrl, nameof(url));

            Scheme = match.Groups["scheme"].Value;
            Path = match.Groups["path"].Value;
            QueryParameters = ParseQueryParameters(match.Groups["query"].Value);
            Fragment = ParseFragment(match.Groups["fragment"].Value);

            // Parse the hostname and port.

            string hostname = match.Groups["hostname"].Value;

            if (hostname.Contains(":")) {

                string[] hostParts = hostname.Split(':');

                Hostname = hostParts[0];

                if (hostParts.Count() > 1 && int.TryParse(hostParts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int port))
                    Port = port;
                else
                    throw new ArgumentException(Properties.ExceptionMessages.MalformedUrl, nameof(url));

            }
            else {

                Hostname = hostname;

            }

            // Parse the credentials.

            var credentials = ParseCredentials(match.Groups["credentials"].Value);

            if (credentials is object) {

                Username = credentials.Item1;
                Password = credentials.Item2;

            }

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            // Add rooth path.

            sb.Append(Root);

            if (!string.IsNullOrEmpty(Path)) {

                if (!Path.StartsWith("/"))
                    sb.Append("/");

                sb.Append(Path);

            }

            // Add query parameters.

            if (QueryParameters is object && QueryParameters.Any()) {

                sb.Append("?");
                sb.Append(string.Join("&", QueryParameters.Select(p => $"{p.Key}={p.Value}")));

            }

            // Add fragment.

            if (!string.IsNullOrWhiteSpace(Fragment)) {

                sb.Append("#");
                sb.Append(Fragment);

            }

            return sb.ToString();

        }

        public static Url Parse(string url) {

            return new Url(url);

        }
        public static bool TryParse(string url, out Url result) {

            try {

                result = Parse(url);

                return true;

            }
            catch (ArgumentException) {

                result = null;

                return false;

            }

        }

        public static string GetHostname(string url) {

            Match hostnameMatch = Regex.Match(url ?? "", @"^(?:[^\s:]+:\/\/|\/\/)?([^\/:]+)");

            if (hostnameMatch.Success)
                return hostnameMatch.Groups[1].Value.TrimEnd('.'); // trim trailing period from fully-qualified domain name

            return string.Empty;

        }
        public static string GetDomainName(string url) {

            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            string hostname = GetHostname(url);
            string[] parts = hostname.Split('.');
            string domain = hostname;

            if (parts.Count() == 4 && parts.All(p => p.All(c => char.IsDigit(c)))) {

                // If the hostname is an IPv4 address, return the IPv4 address as the domain name.

                domain = string.Join(".", parts);

            }
            else if (parts.Length > 2) {

                string suffix = publicSuffixList.Value.Where(x => hostname.EndsWith(x)).FirstOrDefault();

                if (string.IsNullOrEmpty(suffix)) {

                    // If there is no suffix, just return the last two parts of the hostname.

                    domain = string.Join(".", parts.TakeLast(2));

                }
                else {

                    // If there is a suffix, the domain is the part before the suffix.

                    int suffixLength = StringUtilities.Count(suffix, ".");

                    domain = string.Join(".", parts.TakeLast(suffixLength + 1));

                }

            }

            return domain;

        }

        // Private members

        private string scheme;

        private static readonly Lazy<IEnumerable<string>> publicSuffixList = new Lazy<IEnumerable<string>>(BuildPublicSuffixList);

        private string GetHost() {

            StringBuilder sb = new StringBuilder();

            // Add hostname.

            sb.Append(Hostname);

            // Add port.

            if (Port.HasValue) {

                sb.Append(":");
                sb.Append(Port.Value);

            }

            return sb.ToString();

        }
        private string GetRoot() {

            StringBuilder sb = new StringBuilder();

            // Add scheme.

            if (!string.IsNullOrWhiteSpace(Scheme)) {

                sb.Append(Scheme);

                if (!Scheme.EndsWith(":"))
                    sb.Append(":");

            }

            sb.Append("//");

            // Add username/password.

            if (!string.IsNullOrWhiteSpace(Username)) {

                sb.Append(Username);

                // If the user doesn't set a password, don't include one.
                // However, if the password is the empty string, include an empty password (this is what cURL does). 
                // https://catonmat.net/cookbooks/curl/use-basic-http-authentication

                if (!string.IsNullOrEmpty(Password) || Password is object)
                    sb.Append(":");

                if (!string.IsNullOrEmpty(Password))
                    sb.Append(Password);

                sb.Append("@");

            }

            // Add host.

            sb.Append(Host);

            return sb.ToString();

        }

        private static IEnumerable<string> BuildPublicSuffixList() {

            // To determine the suffix, we use the Public Suffix List:
            // https://publicsuffix.org/list/public_suffix_list.dat

            IEnumerable<string> suffixes = Encoding.UTF8.GetString(Properties.Resources.public_suffix_list)
                   .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                   .Where(x => !string.IsNullOrWhiteSpace(x) && !x.StartsWith("//"))
                   .Select(x => '.' + x.TrimStart('*', '!', '.'))
                   .OrderByDescending(x => x.Length);

            return suffixes;

        }

        private static Tuple<string, string> ParseCredentials(string credentialsStr) {

            if (string.IsNullOrEmpty(credentialsStr))
                return null;

            if (credentialsStr.EndsWith("@"))
                credentialsStr = credentialsStr.Substring(0, credentialsStr.Length - 1);

            string[] usernamePassword = credentialsStr.Split(new[] { ':' }, 2);

            if (usernamePassword.Count() < 2)
                return null;

            return Tuple.Create(usernamePassword[0], usernamePassword[1]);

        }
        private static IDictionary<string, string> ParseQueryParameters(string queryParametersStr) {

            queryParametersStr = queryParametersStr ?? "";

            if (queryParametersStr.StartsWith("?"))
                queryParametersStr = queryParametersStr.Substring(1);

            return (queryParametersStr ?? "")
                .Split('&')
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Split(new[] { '=' }, 2))
                .ToDictionary(p => p.First(), p => p.Last());

        }
        private static string ParseFragment(string fragmentStr) {

            if (string.IsNullOrEmpty(fragmentStr))
                return string.Empty;

            if (fragmentStr.StartsWith("#"))
                fragmentStr = fragmentStr.Substring(1);

            return fragmentStr;

        }

        private void SetScheme(string scheme) {

            if (!string.IsNullOrEmpty(scheme)) {

                if (!Regex.Match(scheme, @"^(?<scheme>[\w][\w+-.]+):?$").Success)
                    throw new ArgumentException(Properties.ExceptionMessages.SchemeContainsInvalidCharacters, nameof(scheme));

                if (scheme.EndsWith(":"))
                    scheme = scheme.Substring(0, scheme.Length - 1);

            }

            this.scheme = scheme;

        }

    }

}