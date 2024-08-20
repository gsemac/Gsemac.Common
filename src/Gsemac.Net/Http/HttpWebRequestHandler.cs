using Gsemac.Net.Extensions;
using Gsemac.Polyfills.System.Threading.Tasks;
using System;
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

        protected virtual IHttpWebResponse GetResponse(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return (IHttpWebResponse)base.GetResponse((WebRequest)request, cancellationToken);

        }
        protected virtual Task<IHttpWebResponse> GetResponseAsync(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return TaskEx.Run(() => GetResponse(request, cancellationToken), cancellationToken);

        }
        protected virtual IAsyncResult BeginGetResponse(IHttpWebRequest request, AsyncCallback callback, object state) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return base.BeginGetResponse((WebRequest)request, callback, state);

        }
        protected WebResponse EndGetResponse(IHttpWebRequest request, IAsyncResult asyncResult) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return base.EndGetResponse((WebRequest)request, asyncResult);

        }

        protected sealed internal override WebResponse GetResponse(WebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            IHttpWebRequest httpWebRequest = request.AsHttpWebRequest();

            if (httpWebRequest is object)
                return (WebResponse)GetResponse(httpWebRequest, cancellationToken);

            return base.GetResponse(request, cancellationToken);

        }
        protected sealed internal override Task<WebResponse> GetResponseAsync(WebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            IHttpWebRequest httpWebRequest = request.AsHttpWebRequest();

            if (httpWebRequest is object)
                return GetResponseAsync(httpWebRequest, cancellationToken).ContinueWith(t => (WebResponse)t.Result);

            return base.GetResponseAsync(request, cancellationToken);

        }
        protected internal sealed override IAsyncResult BeginGetResponse(WebRequest request, AsyncCallback callback, object state) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            IHttpWebRequest httpWebRequest = request.AsHttpWebRequest();

            if (httpWebRequest is object)
                return BeginGetResponse(httpWebRequest, callback, state);

            return base.BeginGetResponse(request, callback, state);

        }
        protected internal sealed override WebResponse EndGetResponse(WebRequest request, IAsyncResult asyncResult) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            IHttpWebRequest httpWebRequest = request.AsHttpWebRequest();

            if (httpWebRequest is object)
                return EndGetResponse(httpWebRequest, asyncResult);

            return base.EndGetResponse(request, asyncResult);

        }

    }

}