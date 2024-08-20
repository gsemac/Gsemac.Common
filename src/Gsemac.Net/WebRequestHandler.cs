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

        protected internal virtual WebResponse GetResponse(WebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            return request.GetResponse();

        }
        protected internal virtual Task<WebResponse> GetResponseAsync(WebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return TaskEx.Run(() => GetResponse(request, cancellationToken));

        }
        protected internal virtual IAsyncResult BeginGetResponse(WebRequest request, AsyncCallback callback, object state) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return request.BeginGetResponse(callback, state);

        }
        protected internal virtual WebResponse EndGetResponse(WebRequest request, IAsyncResult asyncResult) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return request.EndGetResponse(asyncResult);

        }

        protected virtual void Dispose(bool disposing) { }

    }

}