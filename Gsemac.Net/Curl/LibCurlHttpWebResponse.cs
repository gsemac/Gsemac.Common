using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Curl {

    public class LibCurlHttpWebResponse :
        HttpWebResponseBase {

        // Public members

        public LibCurlHttpWebResponse(IHttpWebRequest parentRequest, Stream responseStream, CancellationTokenSource cancellationTokenSource) :
            base(parentRequest.RequestUri, responseStream) {

            this.cancellationTokenSource = cancellationTokenSource;
            this.maximumAutomaticRedirections = parentRequest.AllowAutoRedirect ? parentRequest.MaximumAutomaticRedirections : 0;

            Method = parentRequest.Method;
            ProtocolVersion = parentRequest.ProtocolVersion;

            // Read headers from the stream before returning to the caller so our property values are valid.

            ReadHttpHeaders(responseStream);

        }

        public override void Close() {

            // Cancel the thread reading data from curl.

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();

            base.Close();

        }

        // Private members

        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly int maximumAutomaticRedirections;

        private void ReadHttpHeaders(Stream responseStream) {

            using (IHttpResponseReader reader = new HttpResponseReader(responseStream)) {

                try {

                    bool readHeaders = true;
                    int numberOfRedirects = 0;

                    while (readHeaders) {

                        Headers.Clear();

                        IHttpStatusLine statusLine = reader.ReadStatusLine();

                        ProtocolVersion = statusLine.ProtocolVersion;
                        StatusCode = statusLine.StatusCode;
                        StatusDescription = statusLine.StatusDescription;

                        foreach (IHttpHeader header in reader.ReadHeaders())
                            Headers.Add(header.Name, header.Value);

                        readHeaders = !string.IsNullOrEmpty(Headers[HttpResponseHeader.Location]) &&
                            numberOfRedirects++ < maximumAutomaticRedirections;

                    }

                }
                catch (ArgumentException) {

                    throw new ProtocolViolationException("The response was malformed.");

                }

            }

        }

    }

}