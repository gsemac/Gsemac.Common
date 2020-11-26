using Gsemac.IO;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net.Curl {

    public class LibCurlHttpWebRequest :
        HttpWebRequestBase {

        // Public members

        public LibCurlHttpWebRequest(Uri requestUri) :
            base(requestUri) {

            getResponseAsyncDelegate = () => GetResponse();

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

            ConcurrentMemoryStream stream = new ConcurrentMemoryStream() {
                Blocking = true,
                ReadTimeout = Timeout
            };

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task curlTask = Task.Factory.StartNew(() => {

                LibCurl.GlobalInit();

                try {

                    using (CurlEasyHandle easyHandle = LibCurl.EasyInit()) {

                        CurlDataCopier dataCopier = new CurlDataCopier(easyHandle, stream, cancellationToken);

                        LibCurl.EasySetOpt(easyHandle, CurlOption.Url, RequestUri.AbsoluteUri);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.FollowLocation, AllowAutoRedirect ? 1 : 0);
                        LibCurl.EasySetOpt(easyHandle, CurlOption.Timeout, Timeout);

                        if (File.Exists(LibCurl.CABundlePath))
                            LibCurl.EasySetOpt(easyHandle, CurlOption.CaInfo, LibCurl.CABundlePath);

                        LibCurl.Perform(easyHandle);

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

            return getResponseAsyncDelegate.BeginInvoke(callback, state);

        }
        public override WebResponse EndGetResponse(IAsyncResult asyncResult) {

            return getResponseAsyncDelegate.EndInvoke(asyncResult);

        }

        // Private members

        private readonly Func<WebResponse> getResponseAsyncDelegate;
        private MemoryStream requestStream;

    }

}