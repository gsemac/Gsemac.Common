using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Net.Extensions;
using Gsemac.Net.Http;
using Gsemac.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using DecompressionMethods = Gsemac.Polyfills.System.Net.DecompressionMethods;

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

            Stream responseStream = new ProducerConsumerStream(ResponseStreamBufferSize) {
                Blocking = true,
                ReadTimeout = Timeout,
                WriteTimeout = Timeout,
            };

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task curlTask = Task.Factory.StartNew(() => {

                try {

                    GlobalInit();

                    using (CurlEasyHandle easyHandle = LibCurl.EasyInit())
                    using (SList headers = new SList())
                    using (MemoryStream requestStream = new MemoryStream(GetRequestStream(validateMethod: false).ToArray())) {

                        LibCurl.EasySetOpt(easyHandle, CurlOption.Url, base.RequestUri.AbsoluteUri);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.FollowLocation, AllowAutoRedirect ? 1 : 0);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.MaxRedirs, MaximumAutomaticRedirections);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.Timeout, base.Timeout);

                        if ((DecompressionMethods)AutomaticDecompression != DecompressionMethods.None)
                            LibCurl.EasySetOpt(easyHandle, CurlOption.AcceptEncoding, GetAcceptEncoding());

                        if (File.Exists(options.CABundlePath))
                            LibCurl.EasySetOpt(easyHandle, CurlOption.CAInfo, options.CABundlePath);

                        SetCertificateValidationEnabled(easyHandle);
                        SetCookies(easyHandle);
                        SetCredentials(easyHandle);
                        SetHeaders(easyHandle, headers);
                        SetHttpVersion(easyHandle);
                        SetKeepAlive(easyHandle);
                        SetMethod(easyHandle);
                        SetProxy(easyHandle);

                        // Execute the request.

                        using (ICurlDataCopier dataCopier = new CurlDataCopier(requestStream, responseStream, cancellationToken)) {

                            LibCurl.EasySetOpt(easyHandle, CurlOption.HeaderFunction, dataCopier.Header);
                            LibCurl.EasySetOpt(easyHandle, CurlOption.ReadFunction, dataCopier.Read);
                            LibCurl.EasySetOpt(easyHandle, CurlOption.WriteFunction, dataCopier.Write);
                            LibCurl.EasySetOpt(easyHandle, CurlOption.ProgessFunction, dataCopier.Progress);

                            CurlCode resultCode = LibCurl.EasyPerform(easyHandle);

                            if (resultCode != CurlCode.OK)
                                throw new CurlException(resultCode);

                        }

                    }

                }
                finally {

                    // Close the stream to indicate that we're done writing to it, unblocking readers.

                    responseStream.Close();

                    GlobalCleanup();

                }

            }, cancellationToken);

            HaveResponse = true;

            return new CurlHttpWebResponse(this, responseStream, curlTask, cancellationTokenSource);

        }

        // Private members

        private const int ResponseStreamBufferSize = 8192;

        private readonly ICurlWebRequestOptions options;
        private readonly object globalInitLock = new object();

        private void GlobalInit() {

            lock (globalInitLock) {

                // We shouldn't need to check if libcurl is initialized, because it's reference counted and can be called multiple times.
                // However, repeated calls to curl_global_cleanup are crashing my program, so for the time being I'm not pairing it with this.
                // It's up the user to call curl_global_cleanup once when they're done using it. 

                if (!LibCurl.IsInitialized) // Check so ref count will not be increased (only one call to curl_global_cleanup required after multiple requests)
                    LibCurl.GlobalInit();

            }

        }
        private void GlobalCleanup() {

            //LibCurl.GlobalCleanup();

        }

        private string GetAcceptEncoding() {

            List<string> decompressionMethodStrs = new List<string>();
            DecompressionMethods decompressionMethods = (DecompressionMethods)AutomaticDecompression;

            if (decompressionMethods.HasFlag(DecompressionMethods.GZip))
                decompressionMethodStrs.Add("gzip");

            if (decompressionMethods.HasFlag(DecompressionMethods.Deflate))
                decompressionMethodStrs.Add("deflate");

            if (decompressionMethods.HasFlag(DecompressionMethods.Brotli))
                decompressionMethodStrs.Add("br");

            return string.Join(", ", decompressionMethodStrs);

        }
        private CurlHttpVersion GetHttpVersion() {

            if (ProtocolVersion is object) {

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

        private void SetCertificateValidationEnabled(CurlEasyHandle easyHandle) {

            if (!ServicePointManagerUtilities.IsCertificateValidationEnabled()) {

                LibCurl.EasySetOpt(easyHandle, CurlOption.SslVerifyHost, 0);
                LibCurl.EasySetOpt(easyHandle, CurlOption.SslVerifyPeer, 0);

            }

        }
        private void SetCookies(CurlEasyHandle easyHandle) {

            string cookieHeader = CookieContainer?.GetCookieHeader(RequestUri);

            if (!string.IsNullOrEmpty(cookieHeader))
                LibCurl.EasySetOpt(easyHandle, CurlOption.Cookie, cookieHeader);

        }
        private void SetCredentials(CurlEasyHandle easyHandle) {

            if (Credentials is object) {

                string credentialString = Credentials.ToCredentialsString(RequestUri);

                if (!string.IsNullOrEmpty(credentialString))
                    LibCurl.EasySetOpt(easyHandle, CurlOption.UserPwd, credentialString);

            }

        }
        private void SetHeaders(CurlEasyHandle easyHandle, SList headersList) {

            IEnumerable<IHttpHeader> headers = Headers.GetAll();

            foreach (IHttpHeader header in headers)
                headersList.Append($"{header.Name}: {header.Value}");

            if (!Headers.TryGet(HttpRequestHeader.AcceptEncoding, out _) && AutomaticDecompression != System.Net.DecompressionMethods.None) {

                // Adding the Accept-Encoding header manually ensures that it's below the Accept header.
                // See See https://sansec.io/research/http-header-order-is-important

                string acceptEncoding = GetAcceptEncoding();

                if (!string.IsNullOrWhiteSpace(acceptEncoding))
                    headersList.Append($"Accept-Encoding: {acceptEncoding}");

            }

            // Remove default headers unless they were added manually by the user.

            if (!Headers.TryGet(HttpRequestHeader.ContentType, out _))
                headersList.Append($"Content-Type:");

            LibCurl.EasySetOpt(easyHandle, CurlOption.HttpHeader, headersList.Handle);

        }
        private void SetHttpVersion(CurlEasyHandle easyHandle) {

            LibCurl.EasySetOpt(easyHandle, CurlOption.HttpVersion, (int)GetHttpVersion());

        }
        private void SetKeepAlive(CurlEasyHandle easyHandle) {

            LibCurl.EasySetOpt(easyHandle, CurlOption.TcpKeepAlive, KeepAlive ? 1 : 0);

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

            if (Proxy is object && !Proxy.IsBypassed(RequestUri))
                LibCurl.EasySetOpt(easyHandle, CurlOption.Proxy, Proxy.ToProxyString(RequestUri));

        }

    }

}