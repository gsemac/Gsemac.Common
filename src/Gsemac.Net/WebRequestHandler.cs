using Gsemac.Polyfills.System.Threading.Tasks;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net {

    public class WebRequestHandler :
        IDisposable {

        // Public members

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected internal virtual WebResponse Send(WebRequest request, CancellationToken cancellationToken) {

            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            return request.GetResponse();

        }
        protected internal virtual Task<WebResponse> SendAsync(WebRequest request, CancellationToken cancellationToken) {

            return TaskEx.Run(() => Send(request, cancellationToken));

        }

        protected virtual void Dispose(bool disposing) { }

    }

}