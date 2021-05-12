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

        protected internal override WebResponse Send(WebRequest request, CancellationToken cancellationToken) {

            if (InnerHandler is null)
                throw new Exception(Properties.ExceptionMessages.InnerHandlerHasNotBeenSet);

            return InnerHandler.Send(request, cancellationToken);

        }
        protected internal override Task<WebResponse> SendAsync(WebRequest request, CancellationToken cancellationToken) {

            if (InnerHandler is null)
                throw new Exception(Properties.ExceptionMessages.InnerHandlerHasNotBeenSet);

            return InnerHandler.SendAsync(request, cancellationToken);

        }

        protected override void Dispose(bool disposing) {

            if (disposing) {

                InnerHandler?.Dispose();

            }

        }

    }

}