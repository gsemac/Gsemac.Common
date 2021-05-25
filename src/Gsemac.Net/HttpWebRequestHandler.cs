using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net {

    public class HttpWebRequestHandler :
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

            return Polyfills.System.Threading.Tasks.Task.Run(() => Send(request, cancellationToken));

        }

        protected internal override WebResponse Send(WebRequest request, CancellationToken cancellationToken) {

            IHttpWebResponse httpWebResponse;

            switch (request) {

                case HttpWebRequest httpWebRequest:
                    httpWebResponse = Send(new HttpWebRequestWrapper(httpWebRequest), cancellationToken);
                    break;

                case IHttpWebRequest iHttpWebRequest:
                    httpWebResponse = Send(iHttpWebRequest, cancellationToken);
                    break;

                default:
                    return base.Send(request, cancellationToken);

            }

            return (WebResponse)httpWebResponse;

        }
        protected internal override Task<WebResponse> SendAsync(WebRequest request, CancellationToken cancellationToken) {

            Task<IHttpWebResponse> httpWebResponse;

            switch (request) {

                case HttpWebRequest httpWebRequest:
                    httpWebResponse = SendAsync(new HttpWebRequestWrapper(httpWebRequest), cancellationToken);
                    break;

                case IHttpWebRequest iHttpWebRequest:
                    httpWebResponse = SendAsync(iHttpWebRequest, cancellationToken);
                    break;

                default:
                    return base.SendAsync(request, cancellationToken);

            }

            return httpWebResponse.ContinueWith(t => (WebResponse)t.Result);

        }

    }

}