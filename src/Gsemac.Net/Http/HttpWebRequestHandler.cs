using Gsemac.Net.Extensions;
using Gsemac.Polyfills.System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net.Http {

    public abstract class HttpWebRequestHandler :
        DelegatingWebRequestHandler {

        // Protected members

        protected HttpWebRequestHandler() :
            base() {
        }
        protected HttpWebRequestHandler(WebRequestHandler innerHandler) :
            base(innerHandler) {
        }

        protected virtual IHttpWebResponse Send(IHttpWebRequest request, CancellationToken cancellationToken) {

            return (IHttpWebResponse)base.Send((WebRequest)request, cancellationToken);

        }
        protected virtual Task<IHttpWebResponse> SendAsync(IHttpWebRequest request, CancellationToken cancellationToken) {

            return TaskEx.Run(() => Send(request, cancellationToken));

        }

        protected sealed internal override WebResponse Send(WebRequest request, CancellationToken cancellationToken) {

            IHttpWebRequest httpWebRequest = request.AsHttpWebRequest();

            if (httpWebRequest is null) {

                return base.Send(request, cancellationToken);

            }
            else {

                return (WebResponse)Send(httpWebRequest, cancellationToken);

            }

        }
        protected sealed internal override Task<WebResponse> SendAsync(WebRequest request, CancellationToken cancellationToken) {

            IHttpWebRequest httpWebRequest = request.AsHttpWebRequest();

            if (httpWebRequest is null) {

                return base.SendAsync(request, cancellationToken);

            }
            else {

                return SendAsync(httpWebRequest, cancellationToken).ContinueWith(t => (WebResponse)t.Result);

            }

        }

    }

}