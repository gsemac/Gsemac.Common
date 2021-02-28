using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Net.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net.Curl {

    public class CurlHttpWebRequest :
        HttpWebRequestBase {

        // Public members

        public CurlHttpWebRequest(Uri requestUri, ICurlWebRequestOptions options = null) :
            base(requestUri) {

            this.options = options ?? CurlWebRequestOptions.Default;

        }
        public CurlHttpWebRequest(string requestUri, ICurlWebRequestOptions options = null) :
            this(new Uri(requestUri), options) {
        }

        // Methods overidden from WebRequest

        public override WebResponse GetResponse() {

            ConcurrentMemoryStream stream = new ConcurrentMemoryStream() {
                Blocking = true,
                ReadTimeout = Timeout
            };

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task curlTask = Task.Factory.StartNew(() => {

                // We shouldn't need to check if libcurl is initialized, because it's reference counted and can be called multiple times.
                // However, repeated calls to curl_global_cleanup are crashing my program, so for the time being I'm not pairing it with this.
                // It's up the user to call curl_global_cleanup once when they're done using it. 

                if (!LibCurl.IsInitialized) // Check so ref count will not be increased (only one call to curl_global_cleanup required after multiple requests)
                    LibCurl.GlobalInit();

                try {

                    using (CurlEasyHandle easyHandle = LibCurl.EasyInit())
                    using (SList headers = new SList())
                    using (MemoryStream postDataStream = new MemoryStream(GetRequestStream(validateMethod: false).ToArray())) {

                        CurlCallbackHelper dataCopier = new CurlCallbackHelper(stream, postDataStream, cancellationToken);

                        dataCopier.SetCallbacks(easyHandle);

                        LibCurl.EasySetOpt(easyHandle, CurlOption.Url, RequestUri.AbsoluteUri);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.FollowLocation, AllowAutoRedirect ? 1 : 0);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.MaxRedirs, MaximumAutomaticRedirections);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.Timeout, Timeout);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.HttpVersion, (int)GetHttpVersion());

                        if (AutomaticDecompression != DecompressionMethods.None)
                            LibCurl.EasySetOpt(easyHandle, CurlOption.AcceptEncoding, GetAcceptEncoding());

                        LibCurl.EasySetOpt(easyHandle, CurlOption.TcpKeepAlive, KeepAlive ? 1 : 0);

                        if (File.Exists(options.CABundlePath))
                            LibCurl.EasySetOpt(easyHandle, CurlOption.CaInfo, options.CABundlePath);

                        SetCookies(easyHandle);
                        SetCredentials(easyHandle);
                        SetHeaders(easyHandle, headers);
                        SetMethod(easyHandle);
                        SetProxy(easyHandle);

                        // Execute the request.

                        LibCurl.EasyPerform(easyHandle);

                    }

                }
                finally {

                    // Close the stream to indicate that we're done writing to it, unblocking readers.

                    stream.Close();

                    //LibCurl.GlobalCleanup();

                }

            }, cancellationToken);

            HaveResponse = true;

            return new CurlHttpWebResponse(this, stream, cancellationTokenSource);

        }

        // Private members

        private readonly ICurlWebRequestOptions options;

        private string GetAcceptEncoding() {

            List<string> decompressionMethodStrs = new List<string>();

            if (AutomaticDecompression.HasFlag(DecompressionMethods.GZip))
                decompressionMethodStrs.Add("gzip");

            if (AutomaticDecompression.HasFlag(DecompressionMethods.Deflate))
                decompressionMethodStrs.Add("deflate");

            return string.Join(", ", decompressionMethodStrs);

        }
        private CurlHttpVersion GetHttpVersion() {

            if (!(ProtocolVersion is null)) {

                if (ProtocolVersion.Major == 1 && ProtocolVersion.Minor == 0)
                    return CurlHttpVersion.Http10;

                if (ProtocolVersion.Major == 1 && ProtocolVersion.Minor == 1)
                    return CurlHttpVersion.Http11;

                if (ProtocolVersion.Major == 2 && ProtocolVersion.Minor == 0)
                    return CurlHttpVersion.Http2;

                if (ProtocolVersion.Major == 3 && ProtocolVersion.Minor == 0)
                    return CurlHttpVersion.Http3;

            }

            return CurlHttpVersion.None;

        }

        private void SetCookies(CurlEasyHandle easyHandle) {

            string cookieHeader = CookieContainer?.GetCookieHeader(RequestUri);

            if (!string.IsNullOrEmpty(cookieHeader))
                LibCurl.EasySetOpt(easyHandle, CurlOption.Cookie, cookieHeader);

        }
        private void SetCredentials(CurlEasyHandle easyHandle) {

            if (!(Credentials is null)) {

                string credentialString = Credentials.ToCredentialsString(RequestUri);

                if (!string.IsNullOrEmpty(credentialString))
                    LibCurl.EasySetOpt(easyHandle, CurlOption.UserPwd, credentialString);

            }

        }
        private void SetHeaders(CurlEasyHandle easyHandle, SList headers) {

            foreach (string headerName in Headers.AllKeys)
                headers.Append($"{headerName}: {Headers[headerName]}");

            LibCurl.EasySetOpt(easyHandle, CurlOption.HttpHeader, headers.Handle);

        }
        private void SetMethod(CurlEasyHandle easyHandle) {

            if (string.IsNullOrWhiteSpace(Method))
                Method = "GET";

            LibCurl.EasySetOpt(easyHandle, CurlOption.CustomRequest, Method);

            if (Method.Equals("post", StringComparison.OrdinalIgnoreCase))
                LibCurl.EasySetOpt(easyHandle, CurlOption.Post, 1);
            else if (Method.Equals("put", StringComparison.OrdinalIgnoreCase))
                LibCurl.EasySetOpt(easyHandle, CurlOption.Put, 1);
            else if (Method.Equals("head", StringComparison.OrdinalIgnoreCase))
                LibCurl.EasySetOpt(easyHandle, CurlOption.NoBody, 1);

        }
        private void SetProxy(CurlEasyHandle easyHandle) {

            if (!(Proxy is null) && !Proxy.IsBypassed(RequestUri))
                LibCurl.EasySetOpt(easyHandle, CurlOption.Proxy, Proxy.ToProxyString(RequestUri));

        }

    }

}