using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net {

    public abstract class DelegatingWebRequestHandler :
        WebRequestHandler {

        // Public members

        public WebRequestHandler InnerHandler { get; set; }

        // Protected members

        protected DelegatingWebRequestHandler() :
            this(new WebRequestHandler()) {
        }
        protected DelegatingWebRequestHandler(WebRequestHandler innerHandler) {

            InnerHandler = innerHandler;

        }

        protected internal override WebResponse GetResponse(WebRequest request, CancellationToken cancellationToken) {

            if (InnerHandler is null)
                throw new Exception(Properties.ExceptionMessages.InnerHandlerHasNotBeenSet);

            return InnerHandler.GetResponse(request, cancellationToken);

        }
        protected internal override Task<WebResponse> GetResponseAsync(WebRequest request, CancellationToken cancellationToken) {

            if (InnerHandler is null)
                throw new Exception(Properties.ExceptionMessages.InnerHandlerHasNotBeenSet);

            return InnerHandler.GetResponseAsync(request, cancellationToken);

        }
        protected internal override IAsyncResult BeginGetResponse(WebRequest request, AsyncCallback callback, object state) {

            if (InnerHandler is null)
                throw new Exception(Properties.ExceptionMessages.InnerHandlerHasNotBeenSet);

            return InnerHandler.BeginGetResponse(request, callback, state);

        }
        protected internal override WebResponse EndGetResponse(WebRequest request, IAsyncResult asyncResult) {

            if (InnerHandler is null)
                throw new Exception(Properties.ExceptionMessages.InnerHandlerHasNotBeenSet);

            return InnerHandler.EndGetResponse(request, asyncResult);

        }

        protected override void Dispose(bool disposing) {

            if (disposing) {

                InnerHandler?.Dispose();

            }

        }

    }

}