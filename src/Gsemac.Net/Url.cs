using Gsemac.Collections;
using Gsemac.Collections.Extensions;
using Gsemac.IO;
using Gsemac.Text;
using Gsemac.Text.Extensions;
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

        public const char DirectorySeparatorChar = '/';
        public const string SchemeSeparator = "://";

        public string Scheme {
            get => scheme;
            set => SetScheme(value);
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host {
            get => GetHost();
            set => SetHost(value);
        }
        public string Hostname { get; set; }
        public int? Port { get; set; }
        public string Path { get; set; }
        public string Fragment { get; set; }
        public IDictionary<string, string> QueryParameters { get; private set; }

        public Url() :
            this(string.Empty) {
        }
        public Url(string url) {

            Match match = Regex.Match(url ?? string.Empty, @"^(?<scheme>.+?:)?(?:\/\/)?(?<credentials>.+?:.+?@)?(?<host>.+?)?(?<path>\/.*?)?(?<query>\?.+?)?(?<fragment>#.+?)?$");

            if (!match.Success)
                throw new FormatException(Properties.ExceptionMessages.MalformedUrl);

            Scheme = match.Groups["scheme"].Value;
            Path = match.Groups["path"].Value;

            SetQueryParameters(match.Groups["query"].Value);
            SetFragment(match.Groups["fragment"].Value);

            // Parse the hostname and port.

            SetHost(match.Groups["host"].Value);

            // Parse the credentials.

            SetCredentials(match.Groups["credentials"].Value);

        }
        public Url(IUrl url) :
            this(UrlToString(url)) {
        }
        public Url(Uri uri) :
            this(UriToString(uri)) {
        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            // Add rooth path.

            sb.Append(GetRoot());

            if (!string.IsNullOrEmpty(Path)) {

                if (!Path.StartsWith("/"))
                    sb.Append("/");

                sb.Append(Path);

            }

            // Add query parameters.

            if (QueryParameters is object && QueryParameters.Any()) {

                sb.Append("?");
                sb.Append(string.Join("&", QueryParameters.Select(p => !string.IsNullOrEmpty(p.Value) ? $"{p.Key}={Uri.EscapeDataString(p.Value)}" : p.Key)));

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
            catch (FormatException) {

                result = null;

                return false;

            }

        }

        public static bool IsUrl(string value) {
            return PathUtilities.IsUrl(value);
        }

        public static string GetDomainName(string url) {

            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            string hostname = GetHostname(url);

            // Trim trailing periods from fully-qualified domain names.

            if (!string.IsNullOrWhiteSpace(hostname))
                hostname = hostname.TrimEnd('.');

            string[] parts = hostname.Split('.');
            string domain = hostname;

            if (parts.Count() == 4 && parts.All(p => p.All(c => char.IsDigit(c)))) {

                // If the hostname is an IPv4 address, return the IPv4 address as the domain name.

                domain = string.Join(".", parts);

            }
            else if (parts.Length > 2) {

                string suffix = GetPublicSuffixList().Where(x => hostname.EndsWith(x)).FirstOrDefault();

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
        public static string GetHostname(string url) {

            // Returns the same value as GetHost, but with the port number removed.

            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            return GetHost(url)
                .Split(':')
                .FirstOrDefault() ?? string.Empty;

        }
        public static string GetHost(string url) {

            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            Match hostMatch = Regex.Match(url ?? string.Empty, @"^(?:[^\s:]+:\/\/|\/\/)?(?:.+?:.+?@)?(?<hostname>[^\/]+)");

            if (!hostMatch.Success)
                return string.Empty;

            return hostMatch.Groups["hostname"].Value;

        }

        public static string GetQueryParameter(string url, string parameter) {

            if (GetQueryParameters(url).TryGetValue(parameter, out string value))
                return value;

            return string.Empty;

        }
        public static IDictionary<string, string> GetQueryParameters(string url) {

            if (TryParse(url, out Url parsedUrl))
                return parsedUrl.QueryParameters;

            return new Dictionary<string, string>();

        }
        public static string SetQueryParameter(string url, string name, string value) {

            Url parsedUrl = Parse(url);

            parsedUrl.QueryParameters[name] = value;

            return parsedUrl.ToString();

        }
        public static string StripQueryParameters(string url) {

            if (string.IsNullOrWhiteSpace(url))
                return url;

            return Regex.Replace(url, @"\?.+?(?=#|$)", string.Empty);

        }
        public static string StripFragment(string url) {

            if (string.IsNullOrWhiteSpace(url))
                return url;

            int fragmentIndex = url.IndexOf("#");

            return fragmentIndex < 0 ?
                url :
                url.Substring(0, fragmentIndex);

        }

        public static string Combine(params string[] parts) {

            if (parts is null)
                throw new ArgumentNullException(nameof(parts));

            return Combine((IEnumerable<string>)parts);

        }
        public static string Combine(IEnumerable<string> parts) {

            if (parts is null)
                throw new ArgumentNullException(nameof(parts));

            if (!parts.Any())
                return string.Empty;

            if (parts.Count() < 2)
                return parts.First();

            string leftPart = parts.First();
            string rightPart = parts.Skip(1).First();

            string result = leftPart;

            if (string.IsNullOrWhiteSpace(leftPart) || PathUtilities.IsPathRooted(rightPart, new PathInfo() { IsUrl = true })) {

                result = rightPart;

            }
            else if (!string.IsNullOrWhiteSpace(rightPart)) {

                // If the path we're combining is a relative path beginning with a dot, trim the dot part.
                // The path will still be treated as a relative path.

                if (rightPart.StartsWith("./"))
                    rightPart = rightPart.TrimStart("./");

                string scheme = PathUtilities.GetScheme(leftPart);

                if (!string.IsNullOrWhiteSpace(scheme) && rightPart.StartsWith("//") && !rightPart.Equals("//")) {

                    // Prepend the new path with the scheme.

                    result = $"{scheme}:{rightPart}";

                }
                else if (rightPart.StartsWith("/")) {

                    // Make the new path relative to the root.

                    string rootUrl = PathUtilities.GetRootPath(leftPart, new PathInfo() { IsUrl = true });
                    string relativePath = PathUtilities.TrimLeftDirectorySeparators(rightPart);

                    result = $"{rootUrl}/{relativePath}";

                }
                else if (leftPart.EndsWith("/")) {

                    // Append the new path to the current path.

                    result = $"{leftPart}{rightPart}";

                }
                else if (rightPart.StartsWith("?")) {

                    // Replace the query parameters in the URL.

                    result = $"{StripQueryParameters(leftPart)}{rightPart}";

                }
                else if (rightPart.StartsWith("#")) {

                    // Replace the fragment in the URL.

                    result = $"{StripFragment(leftPart)}{rightPart}";

                }
                else {

                    // Replace the current directory with the new path.

                    string parentUrl = PathUtilities.GetParentPath(leftPart);

                    if (string.IsNullOrEmpty(parentUrl))
                        parentUrl = leftPart;

                    if (parentUrl.EndsWith("/"))
                        parentUrl = parentUrl.Substring(0, parentUrl.Length - 1);

                    result = $"{parentUrl}/{rightPart}";

                }

            }

            return Combine(new[] { (result ?? "").Trim() }.Concat(parts.Skip(2)));

        }

        // Private members

        private string scheme;

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

                sb.Append(Scheme.TrimEnd(':'));
                sb.Append(":");

            }

            sb.Append("//");

            // Add username/password.

            if (!string.IsNullOrWhiteSpace(UserName)) {

                sb.Append(UserName);

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

            string rootString = sb.ToString();

            if (rootString.Equals("//"))
                rootString = string.Empty;

            return rootString;

        }

        private void SetScheme(string scheme) {

            if (!string.IsNullOrEmpty(scheme)) {

                if (!Regex.Match(scheme, @"^(?<scheme>[\w][\w+-.]+):?$").Success)
                    throw new ArgumentException(Properties.ExceptionMessages.SchemeContainsInvalidCharacters, nameof(scheme));

                if (scheme.EndsWith(":"))
                    scheme = scheme.TrimEnd(':');

            }

            this.scheme = scheme;

        }
        private void SetHost(string host) {

            if (!string.IsNullOrEmpty(host) && host.Contains(":")) {

                string[] hostParts = host.Split(':');

                Hostname = hostParts[0];

                if (hostParts.Count() > 1 && int.TryParse(hostParts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int port))
                    Port = port;
                else
                    throw new ArgumentException(Properties.ExceptionMessages.MalformedUrl, nameof(host));

            }
            else {

                Hostname = host;
                Port = null;

            }

        }
        private void SetCredentials(string credentialsStr) {

            if (!string.IsNullOrEmpty(credentialsStr)) {

                if (credentialsStr.EndsWith("@"))
                    credentialsStr = credentialsStr.Substring(0, credentialsStr.Length - 1);

                string[] usernamePassword = credentialsStr.Split(new[] { ':' }, 2);

                if (usernamePassword.Count() == 2) {

                    this.UserName = usernamePassword[0];
                    this.Password = usernamePassword[1];

                }

            }


        }
        private void SetQueryParameters(string queryParametersStr) {

            // Query parameter names are case-sensitive, so do not alter their casing.

            queryParametersStr = queryParametersStr ?? "";

            if (queryParametersStr.StartsWith("?"))
                queryParametersStr = queryParametersStr.Substring(1);

            IEnumerable<KeyValuePair<string, string>> keyValuePairs = (queryParametersStr ?? "")
                .Split('&')
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Split(new[] { '=' }, 2))
                .Select(pair => new KeyValuePair<string, string>(pair.First(), Uri.UnescapeDataString(pair.Skip(1).FirstOrDefault() ?? string.Empty)));

            QueryParameters = new OrderedDictionary<string, string>(keyValuePairs);

        }
        private void SetFragment(string fragmentStr) {

            if (!string.IsNullOrEmpty(fragmentStr)) {

                if (fragmentStr.StartsWith("#"))
                    fragmentStr = fragmentStr.Substring(1);

                this.Fragment = fragmentStr;

            }

        }

        private static IEnumerable<string> GetPublicSuffixList() {

            // To determine the suffix, we use the Public Suffix List:
            // https://publicsuffix.org/list/public_suffix_list.dat

            IPublicSuffixListProvider provider = PublicSuffixListProvider.Default ??
                        new ResourcePublicSuffixListProvider();

            return provider.GetList();

        }
        private static string UriToString(Uri uri) {

            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            return uri.AbsoluteUri;

        }
        private static string UrlToString(IUrl url) {

            if (url is null)
                throw new ArgumentNullException(nameof(url));

            return url.ToString();

        }

    }

}