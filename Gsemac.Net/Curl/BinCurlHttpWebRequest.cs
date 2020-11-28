using Gsemac.Net.Extensions;
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

        private string GetPostData() {

            if (requestStream is null)
                return string.Empty;

            return Encoding.UTF8.GetString(requestStream.ToArray());

        }
        private string GetCurlArguments() {

            return new CurlCommandLineArgumentsBuilder()
                .WithHeaderOutput()
                .WithConsoleOutput()
                .WithAutomaticRedirect(AllowAutoRedirect ? MaximumAutomaticRedirections : 0)
                .WithAutomaticDecompression(AutomaticDecompression)
                .WithHttpVersion(ProtocolVersion)
                .WithKeepAlive(KeepAlive)
                .WithConnectTimeout(Timeout)
                .WithHeaders(Headers)
                .WithCookies(CookieContainer, RequestUri)
                .WithPostData(GetPostData())
                .WithProxy(Proxy, RequestUri)
                .WithCredentials(Credentials, RequestUri)
                .WithUri(RequestUri)
                .ToString();

        }

    }

}