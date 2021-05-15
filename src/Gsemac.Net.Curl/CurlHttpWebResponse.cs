using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net.Curl {

    internal class CurlHttpWebResponse :
        CurlHttpWebResponseBase {

        // Public members

        internal CurlHttpWebResponse(IHttpWebRequest parentRequest, Stream responseStream, Task task, CancellationTokenSource taskCancellationTokenSource) :
            base(parentRequest, responseStream, () => task.Exception?.InnerExceptions.First()) {

            this.taskCancellationTokenSource = taskCancellationTokenSource;

            ReadHeadersFromResponseStream();

        }

        public override void Close() {

            // Cancel the thread reading data from curl.

            taskCancellationTokenSource.Cancel();
            taskCancellationTokenSource.Dispose();

            base.Close();

        }

        // Private members

        private readonly CancellationTokenSource taskCancellationTokenSource;

    }

}