using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Gsemac.Net.Curl {

    public class BinCurlHttpWebRequest :
        HttpWebRequestBase {

        // Public members

        /// <summary>
        /// Returns the arguments that are passed to curl.
        /// </summary>
        public string CurlArguments => GetCurlArguments();

        public BinCurlHttpWebRequest(Uri requestUri) :
            this(requestUri, LibCurl.CurlExecutablePath) {

            getResponseAsyncDelegate = () => GetResponse();

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BinCurlHttpWebRequest"/> class.
        /// </summary>
        /// <param name="curlExecutablePath">Path to the curl executable.</param>
        /// <param name="requestUri">URI to request.</param>
        public BinCurlHttpWebRequest(Uri requestUri, string curlExecutablePath) :
            base(requestUri) {

            this.curlExecutablePath = curlExecutablePath;

        }

        // Methods overidden from WebRequest

        public override Stream GetRequestStream() {

            if (string.IsNullOrEmpty(Method))
                Method = "POST";

            if (!Method.Equals("POST", StringComparison.OrdinalIgnoreCase) && !Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
                throw new ProtocolViolationException("Method must be POST or PUT.");

            if (requestStream is null)
                requestStream = new MemoryStream();

            return requestStream;

        }
        public override WebResponse GetResponse() {

            BinCurlProcessStream stream = new BinCurlProcessStream(curlExecutablePath, CurlArguments) {
                ReadTimeout = ReadWriteTimeout,
                WriteTimeout = ReadWriteTimeout
            };

            HaveResponse = true;

            return new BinCurlHttpWebResponse(RequestUri, stream, ProtocolVersion, Method);

        }
        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state) {

            return getResponseAsyncDelegate.BeginInvoke(callback, state);

        }
        public override WebResponse EndGetResponse(IAsyncResult asyncResult) {

            return getResponseAsyncDelegate.EndInvoke(asyncResult);

        }

        // Private members

        private readonly string curlExecutablePath;
        private readonly Func<WebResponse> getResponseAsyncDelegate;
        private MemoryStream requestStream;

        string GetCurlArguments() {

            List<string> arguments = new List<string> {
                "--include",    // include response headers in output
                "--output - ",  // output binary data directly to standard output
            };

            if (AllowAutoRedirect) {

                arguments.Add("--location");
                arguments.Add(string.Format("--max-redirs {0}", MaximumAutomaticRedirections));

            }

            if (AutomaticDecompression != DecompressionMethods.None)
                arguments.Add("--compressed");

            if (!KeepAlive)
                arguments.Add("--no-keepalive");

            if (ProtocolVersion != null)
                if (ProtocolVersion.Major > 1)
                    arguments.Add(string.Format("--http{0}", ProtocolVersion.Major));
                else
                    arguments.Add(string.Format("--http{0}.{1}", ProtocolVersion.Major, ProtocolVersion.Minor));

            if (Timeout != System.Threading.Timeout.Infinite)
                arguments.Add(string.Format("--connect-timeout {0}", Timeout / 1000.0));

            // Add headers.

            arguments.AddRange(HeadersToCurlArguments());

            // Add cookies.

            arguments.Add(CookiesToCurlArgument());

            // Add method.

            arguments.Add(MethodToCurlArgument());

            // Add request data.

            arguments.Add(DataToCurlArgument());

            // Add proxy.

            arguments.Add(ProxyToCurlArgument());

            // Add basic auth credentials.

            arguments.Add(CredentialsToCurlArgument());

            string argumentString = string.Format("{0} \"{1}\"", string.Join(" ", arguments.Where(x => !string.IsNullOrEmpty(x))), EscapeCurlArgument(RequestUri.AbsoluteUri));

            return argumentString;

        }
        List<string> HeadersToCurlArguments() {

            List<string> result = new List<string>();

            for (int i = 0; i < Headers.Count; ++i) {

                string header = Headers.GetKey(i);

                foreach (string value in Headers.GetValues(i))
                    result.Add(string.Format("--header \"{0}: {1}\"", EscapeCurlArgument(header), EscapeCurlArgument(Headers[header])));

            }

            if (AutomaticDecompression != DecompressionMethods.None) {

                List<string> headerValues = new List<string>();

                if (AutomaticDecompression.HasFlag(DecompressionMethods.Deflate))
                    headerValues.Add("deflate");

                if (AutomaticDecompression.HasFlag(DecompressionMethods.GZip))
                    headerValues.Add("gzip");

                string headerValue = string.Join(",", headerValues);

                if (!string.IsNullOrEmpty(headerValue))
                    result.Add(string.Format("--header \"{0}: {1}\"", "Accept-Encoding", headerValue));

            }

            return result;

        }
        string CookiesToCurlArgument() {

            if (CookieContainer is null)
                return string.Empty;

            List<string> cookieStrings = new List<string>();

            foreach (Cookie cookie in CookieContainer.GetCookies(RequestUri))
                cookieStrings.Add(cookie.ToString());

            string argumentString = string.Format("--cookie \"{0}\"", EscapeCurlArgument(string.Join(";", cookieStrings)));

            return cookieStrings.Count <= 0 ? string.Empty : argumentString;

        }
        string MethodToCurlArgument() {

            if (string.IsNullOrEmpty(Method))
                Method = "GET";

            switch (Method.ToLower()) {

                case "get": // no action required
                    return string.Empty;

                case "head": // make HEAD request + redirect stderr to stdout so we can read it via the ProcessStream
                    return "--head --stderr -";

                case "post":
                    return "-X POST";

                case "put":
                    return "-X PUT";

                default:
                    return string.Format("-X {0}", Method);

            }

        }
        string DataToCurlArgument() {

            if (requestStream is null)
                return string.Empty;

            string dataString = Encoding.UTF8.GetString(requestStream.ToArray());

            return string.Format("--data \"{0}\"", EscapeCurlArgument(dataString));

        }
        string ProxyToCurlArgument() {

            // Argument should be of the following form:
            // -x, --proxy <[protocol://][user:password@]proxyhost[:port]>

            if (Proxy is null || Proxy.IsBypassed(RequestUri))
                return string.Empty;

            Uri proxyUri = Proxy.GetProxy(RequestUri);

            StringBuilder sb = new StringBuilder();

            sb.Append("--proxy ");
            sb.Append("\"");

            sb.Append(proxyUri.GetLeftPart(UriPartial.Scheme));

            if (Proxy.Credentials != null) {

                NetworkCredential credentials = Proxy.Credentials.GetCredential(RequestUri, string.Empty);

                if (credentials != null) {

                    sb.Append(EscapeCurlArgument(credentials.UserName));
                    sb.Append(":");
                    sb.Append(EscapeCurlArgument(credentials.Password));
                    sb.Append("@");

                }

            }

            sb.Append(proxyUri.Host);

            if (!proxyUri.IsDefaultPort)
                sb.Append(string.Format(":{0}", proxyUri.Port));

            sb.Append("/");

            sb.Append("\"");

            return sb.ToString();

        }
        string CredentialsToCurlArgument() {

            // Argument should be of the following form:
            // -u username:password

            StringBuilder sb = new StringBuilder();

            if (Credentials != null) {

                NetworkCredential credentials = Proxy.Credentials.GetCredential(RequestUri, string.Empty);

                if (credentials != null) {

                    sb.Append("-u ");
                    sb.Append("\"");

                    sb.Append(EscapeCurlArgument(credentials.UserName));
                    sb.Append(":");
                    sb.Append(EscapeCurlArgument(credentials.Password));

                    sb.Append("\"");

                }

            }

            return sb.ToString();

        }
        public string EscapeCurlArgument(string input) {

            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace("\"", @"\""");

        }

    }

}