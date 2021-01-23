using Gsemac.Core;
using Gsemac.Net.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Curl {

    public class CurlCommandLineArgumentsBuilder :
        CommandLineArgumentsBuilder {

        // Public members

        public new CurlCommandLineArgumentsBuilder WithArgument(string argumentValue) {

            base.AddArgument(argumentValue);

            return this;

        }
        public new CurlCommandLineArgumentsBuilder WithArgument(string argumentName, string argumentValue) {

            base.AddArgument(argumentName, argumentValue);

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithAutomaticDecompression(DecompressionMethods decompressionMethods) {

            if (decompressionMethods != DecompressionMethods.None) {

                AddArgument("--compressed");

                List<string> headerValues = new List<string>();

                if (decompressionMethods.HasFlag(DecompressionMethods.GZip))
                    headerValues.Add("gzip");

                if (decompressionMethods.HasFlag(DecompressionMethods.Deflate))
                    headerValues.Add("deflate");

                string headerValue = string.Join(",", headerValues);

                if (!string.IsNullOrEmpty(headerValue))
                    AddArgument("--header", $"Accept-Encoding: {headerValue}");

            }

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithAutomaticRedirect(int maxRedirections) {

            if (maxRedirections > 0) {

                AddArgument("--location");
                AddArgument("--max-redirs", maxRedirections.ToString(CultureInfo.InvariantCulture));

            }

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithCookies(CookieContainer cookieContainer, Uri requestUri = null) {

            if (requestUri is null)
                requestUri = uri;

            if (!(cookieContainer is null)) {

                List<string> cookieStrings = new List<string>();

                foreach (Cookie cookie in cookieContainer.GetCookies(requestUri))
                    cookieStrings.Add(cookie.ToString());

                if (cookieStrings.Count > 0)
                    AddArgument("--cookie", string.Join(";", cookieStrings));

            }

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithConnectTimeout(int timeoutSeconds) {

            if (timeoutSeconds != Timeout.Infinite)
                AddArgument("--connect-timeout", (timeoutSeconds / 1000.0).ToString(CultureInfo.InvariantCulture));

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithConsoleOutput() => WithArgument("--output", "-"); // output binary data directly to standard output
        public CurlCommandLineArgumentsBuilder WithCredentials(ICredentials credentials, Uri requestUri = null) {

            // Argument should be of the following form:
            // -u username:password

            if (requestUri is null)
                requestUri = uri;

            if (!(credentials is null))
                AddArgument("-u", credentials.ToCredentialsString(requestUri));

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithHeaderOutput() => WithArgument("--include"); // include response headers in output
        public CurlCommandLineArgumentsBuilder WithHeaders(WebHeaderCollection headers) {

            for (int i = 0; i < headers.Count; ++i) {

                string header = headers.GetKey(i);

                foreach (string value in headers.GetValues(i))
                    AddArgument("--header", $"{header}: {headers[header]}");

            }

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithHttpVersion(System.Version version) {

            if (version != null) {

                if (version.Major > 1)
                    AddArgument(string.Format("--http{0}", version.Major));
                else
                    AddArgument(string.Format("--http{0}.{1}", version.Major, version.Minor));

            }

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithKeepAlive(bool keepAlive) {

            if (!keepAlive)
                AddArgument("--no-keepalive");

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithMethod(string method) {

            if (string.IsNullOrWhiteSpace(method))
                method = "GET";

            switch (method.ToLowerInvariant()) {

                case "get": // no action required
                    break;

                case "head": // make HEAD request + redirect stderr to stdout so we can read it via the ProcessStream

                    AddArgument("--head");
                    AddArgument("--stderr", "-");

                    break;

                case "post":
                    AddArgument("-X", "POST");
                    break;

                case "put":
                    AddArgument("-X", "PUT");
                    break;

                default:
                    AddArgument("-X", method);
                    break;

            }

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithPostData(string postData) {

            if (!string.IsNullOrWhiteSpace(postData))
                AddArgument("--data", postData);

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithProxy(IWebProxy proxy, Uri requestUri = null) {

            // Argument should be of the following form:
            // -x, --proxy <[protocol://][user:password@]proxyhost[:port]>

            if (requestUri is null)
                requestUri = uri;

            if (!(proxy is null) && !proxy.IsBypassed(requestUri))
                AddArgument("--proxy", proxy.ToProxyString(requestUri));

            return this;

        }
        public CurlCommandLineArgumentsBuilder WithUri(Uri uri) {

            this.uri = uri;

            AddArgument(uri.AbsoluteUri);

            return this;

        }

        // Private members

        private Uri uri = new Uri("http://example.com");

    }

}