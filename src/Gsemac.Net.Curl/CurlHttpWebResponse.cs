using Gsemac.Net.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net.Curl {

    [ResponseStreamAlreadyDecompressed]
    internal sealed class CurlHttpWebResponse :
        CurlHttpWebResponseBase {

        // Public members

        internal CurlHttpWebResponse(IHttpWebRequest originatingRequest, Stream responseStream, Task curlTask, CancellationTokenSource cancellationTokenSource) :
            base(originatingRequest, responseStream, () => curlTask.Exception?.InnerExceptions.First()) {

            if (originatingRequest is null)
                throw new ArgumentNullException(nameof(originatingRequest));

            if (responseStream is null)
                throw new ArgumentNullException(nameof(responseStream));

            if (cancellationTokenSource is null)
                throw new ArgumentNullException(nameof(cancellationTokenSource));

            this.cancellationTokenSource = cancellationTokenSource;

            ReadHttpHeadersFromStream();

        }

        public override void Close() {

            if (!isClosed) {

                // Cancel the Curl reading thread.

                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();

                isClosed = true;

            }

            base.Close();

        }

        // Private members

        private bool isClosed;
        private readonly CancellationTokenSource cancellationTokenSource;

    }

}