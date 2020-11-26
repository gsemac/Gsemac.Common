using Gsemac.IO;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Curl {

    public class LibCurlHttpWebResponse :
        HttpWebResponseBase {

        // Public members

        public LibCurlHttpWebResponse(Uri responseUri, Stream responseStream, CancellationTokenSource cancellationTokenSource) :
            base(responseUri, responseStream) {

            this.cancellationTokenSource = cancellationTokenSource;

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

        private void ReadHttpHeaders(Stream responseStream) {

            using (IHttpResponseReader reader = new HttpResponseReader(responseStream)) {

                try {

                    IHttpStatusLine statusLine = reader.ReadStatusLine();

                    ProtocolVersion = statusLine.ProtocolVersion;
                    StatusCode = statusLine.StatusCode;
                    StatusDescription = statusLine.StatusDescription;

                    foreach (IHttpHeader header in reader.ReadHeaders())
                        Headers.Add(header.Name, header.Value);

                }
                catch (ArgumentException) {

                    throw new ProtocolViolationException("The response was malformed.");

                }

            }

        }

    }

}