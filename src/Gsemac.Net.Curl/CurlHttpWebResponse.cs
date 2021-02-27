using System.IO;
using System.Threading;

namespace Gsemac.Net.Curl {

    internal class CurlHttpWebResponse :
        CurlHttpWebResponseBase {

        // Public members

        public CurlHttpWebResponse(IHttpWebRequest parentRequest, Stream responseStream, CancellationTokenSource cancellationTokenSource) :
            base(parentRequest, responseStream) {

            this.cancellationTokenSource = cancellationTokenSource;

        }

        public override void Close() {

            // Cancel the thread reading data from curl.

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();

            base.Close();

        }

        // Private members

        private readonly CancellationTokenSource cancellationTokenSource;

    }

}