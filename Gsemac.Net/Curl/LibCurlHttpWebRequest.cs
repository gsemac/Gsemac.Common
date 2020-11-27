using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net.Curl {

    public class LibCurlHttpWebRequest :
        HttpWebRequestBase {

        // Public members

        public LibCurlHttpWebRequest(Uri requestUri) :
            base(requestUri) {

            getResponseDelegate = new GetResponseDelegate(GetResponse);

        }

        // Methods overidden from WebRequest

        public override Stream GetRequestStream() {

            if (string.IsNullOrEmpty(Method))
                Method = "POST";

            if (!Method.Equals("POST", StringComparison.OrdinalIgnoreCase) && !Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
                throw new ProtocolViolationException("Method must be POST or PUT.");

            return requestStream;

        }
        public override WebResponse GetResponse() {

            ConcurrentMemoryStream stream = new ConcurrentMemoryStream() {
                Blocking = true,
                ReadTimeout = Timeout
            };

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task curlTask = Task.Factory.StartNew(() => {

                LibCurl.GlobalInit();

                try {

                    using (CurlEasyHandle easyHandle = LibCurl.EasyInit())
                    using (SList headers = new SList()) {

                        CurlDataCopier dataCopier = new CurlDataCopier(easyHandle, stream, cancellationToken);

                        LibCurl.EasySetOpt(easyHandle, CurlOption.Url, RequestUri.AbsoluteUri);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.FollowLocation, AllowAutoRedirect ? 1 : 0);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.MaxRedirs, MaximumAutomaticRedirections);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.Timeout, Timeout);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.CustomRequest, Method);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.HttpVersion, (int)GetHttpVersion());

                        if (AutomaticDecompression != DecompressionMethods.None)
                            LibCurl.EasySetOpt(easyHandle, CurlOption.AcceptEncoding, GetAcceptEncoding());

                        LibCurl.EasySetOpt(easyHandle, CurlOption.TcpKeepAlive, KeepAlive ? 1 : 0);

                        if (File.Exists(LibCurl.CABundlePath))
                            LibCurl.EasySetOpt(easyHandle, CurlOption.CaInfo, LibCurl.CABundlePath);

                        // Copy headers.

                        foreach (string headerName in Headers.AllKeys)
                            headers.Append($"{headerName}: {Headers[headerName]}");

                        LibCurl.EasySetOpt(easyHandle, CurlOption.HttpHeader, headers.Handle.DangerousGetHandle());

                        // Copy cookies.

                        string cookieHeader = CookieContainer?.GetCookieHeader(RequestUri);

                        if (!string.IsNullOrEmpty(cookieHeader))
                            LibCurl.EasySetOpt(easyHandle, CurlOption.Cookie, cookieHeader);

                        // Add request data (i.e. POST/PUT data).

                        string requestData = GetRequestData();

                        if (requestData.Length > 0)
                            LibCurl.EasySetOpt(easyHandle, CurlOption.PostFields, requestData);

                        // Execute the request.

                        LibCurl.EasyPerform(easyHandle);

                        // Close the stream to indicate that we're done writing to it, unblocking readers.

                        stream.Close();

                    }

                }
                finally {

                    LibCurl.GlobalCleanup();

                }

            }, cancellationToken);

            HaveResponse = true;

            return new LibCurlHttpWebResponse(RequestUri, stream, cancellationTokenSource);

        }
        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state) {

            return getResponseDelegate.BeginInvoke(callback, state);

        }
        public override WebResponse EndGetResponse(IAsyncResult asyncResult) {

            return getResponseDelegate.EndInvoke(asyncResult);

        }

        // Private members

        private delegate WebResponse GetResponseDelegate();

        private readonly GetResponseDelegate getResponseDelegate;
        private readonly MemoryStream requestStream = new MemoryStream();

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
        private string GetRequestData() {

            string requestData = Encoding.ASCII.GetString(requestStream.ToArray());

            requestStream.Dispose();

            return requestData;

        }

    }

}