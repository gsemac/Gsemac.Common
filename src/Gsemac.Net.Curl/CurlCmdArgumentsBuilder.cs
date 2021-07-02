using Gsemac.Core;
using Gsemac.Net.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Curl {

    public class CurlCmdArgumentsBuilder :
        CmdArgumentsBuilderBase<CurlCmdArgumentsBuilder> {

        // Public members

        public CurlCmdArgumentsBuilder WithAutomaticDecompression(DecompressionMethods decompressionMethods) {

            if (decompressionMethods != DecompressionMethods.None) {

                WithArgument("--compressed");

                List<string> headerValues = new List<string>();

                if (decompressionMethods.HasFlag(DecompressionMethods.GZip))
                    headerValues.Add("gzip");

                if (decompressionMethods.HasFlag(DecompressionMethods.Deflate))
                    headerValues.Add("deflate");

                string headerValue = string.Join(",", headerValues);

                if (!string.IsNullOrEmpty(headerValue))
                    WithArgument("--header", $"Accept-Encoding: {headerValue}");

            }

            return this;

        }
        public CurlCmdArgumentsBuilder WithAutomaticRedirect(int maxRedirections) {

            if (maxRedirections > 0) {

                WithArgument("--location");
                WithArgument("--max-redirs", maxRedirections.ToString(CultureInfo.InvariantCulture));

            }

            return this;

        }
        public CurlCmdArgumentsBuilder WithCertificateValidation(bool certificateValidationEnabled) {

            if (!certificateValidationEnabled)
                WithArgument("--insecure");

            return this;

        }
        public CurlCmdArgumentsBuilder WithCookies(CookieContainer cookieContainer, Uri requestUri = null) {

            if (requestUri is null)
                requestUri = uri;

            if (!(cookieContainer is null)) {

                List<string> cookieStrings = new List<string>();

                foreach (Cookie cookie in cookieContainer.GetCookies(requestUri))
                    cookieStrings.Add(cookie.ToString());

                if (cookieStrings.Count > 0)
                    WithArgument("--cookie", string.Join(";", cookieStrings));

            }

            return this;

        }
        public CurlCmdArgumentsBuilder WithConnectTimeout(int timeoutSeconds) {

            if (timeoutSeconds != Timeout.Infinite)
                WithArgument("--connect-timeout", (timeoutSeconds / 1000.0).ToString(CultureInfo.InvariantCulture));

            return this;

        }
        public CurlCmdArgumentsBuilder WithConsoleOutput() => WithArgument("--output", "-"); // output binary data directly to standard output
        public CurlCmdArgumentsBuilder WithCredentials(ICredentials credentials, Uri requestUri = null) {

            // Argument should be of the following form:
            // -u username:password

            if (requestUri is null)
                requestUri = uri;

            if (!(credentials is null))
                WithArgument("-u", credentials.ToCredentialsString(requestUri));

            return this;

        }
        public CurlCmdArgumentsBuilder WithHeaderOutput() => WithArgument("--include"); // include response headers in output
        public CurlCmdArgumentsBuilder WithHeaders(WebHeaderCollection headers) {

            for (int i = 0; i < headers.Count; ++i) {

                string header = headers.GetKey(i);

                foreach (string value in headers.GetValues(i))
                    WithArgument("--header", $"{header}: {headers[header]}");

            }

            return this;

        }
        public CurlCmdArgumentsBuilder WithHttpVersion(System.Version version) {

            if (version != null) {

                if (version.Major > 1)
                    WithArgument(string.Format("--http{0}", version.Major));
                else
                    WithArgument(string.Format("--http{0}.{1}", version.Major, version.Minor));

            }

            return this;

        }
        public CurlCmdArgumentsBuilder WithKeepAlive(bool keepAlive) {

            if (!keepAlive)
                WithArgument("--no-keepalive");

            return this;

        }
        public CurlCmdArgumentsBuilder WithMethod(string method) {

            if (string.IsNullOrWhiteSpace(method))
                method = "GET";

            switch (method.ToLowerInvariant()) {

                case "get": // no action required
                    break;

                case "head": // make HEAD request + redirect stderr to stdout so we can read it via the ProcessStream

                    WithArgument("--head");
                    WithArgument("--stderr", "-");

                    break;

                case "post":
                    WithArgument("-X", "POST");
                    break;

                case "put":
                    WithArgument("-X", "PUT");
                    break;

                default:
                    WithArgument("-X", method);
                    break;

            }

            return this;

        }
        public CurlCmdArgumentsBuilder WithPostData(string postData) {

            if (!string.IsNullOrWhiteSpace(postData))
                WithArgument("--data", postData);

            return this;

        }
        public CurlCmdArgumentsBuilder WithProxy(IWebProxy proxy, Uri requestUri = null) {

            // Argument should be of the following form:
            // -x, --proxy <[protocol://][user:password@]proxyhost[:port]>

            if (requestUri is null)
                requestUri = uri;

            if (!(proxy is null) && !proxy.IsBypassed(requestUri))
                WithArgument("--proxy", proxy.ToProxyString(requestUri));

            return this;

        }
        public CurlCmdArgumentsBuilder WithUri(Uri uri) {

            this.uri = uri;

            WithArgument(uri.AbsoluteUri);

            return this;

        }

        // Private members

        private Uri uri = new Uri("http://example.com");

    }

}