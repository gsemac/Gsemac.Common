using Gsemac.Net.Extensions;
using System;
using System.Net;
using System.Threading;

namespace Gsemac.Net {

    public abstract class WebClientBase :
        WebClient,
        IWebClient {

        // Protected members

        protected WebClientBase() :
            this(HttpWebRequestFactory.Default) {
        }
        protected WebClientBase(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

            // Replace the default proxy with a placeholder, so we can detect if the user has changed the proxy property.
            // This would not be necessary if we could override the Proxy property, but alas, we cannot.

            Proxy = new PlaceholderWebProxy(Proxy);

        }
        protected WebClientBase(WebRequestHandler webRequestHandler) :
            this(HttpWebRequestFactory.Default, webRequestHandler) {
        }
        protected WebClientBase(IHttpWebRequestFactory webRequestFactory, WebRequestHandler webRequestHandler) :
            this(webRequestFactory) {

            this.webRequestHandler = webRequestHandler;

        }

        protected override WebRequest GetWebRequest(Uri address) {

            // The only way to get the method is to call base.GetWebRequest() and copy its properties.
            // Note that calling this method also clears most headers from the WebClient.

            WebRequest baseWebRequest = base.GetWebRequest(address);

            if (baseWebRequest is HttpWebRequest baseHttpWebRequest) {

                IHttpWebRequest httpWebRequest = webRequestFactory.Create(address);

                if (baseHttpWebRequest.Credentials is object)
                    httpWebRequest.Credentials = baseHttpWebRequest.Credentials;

                httpWebRequest.Method = baseHttpWebRequest.Method;

                if (!(baseHttpWebRequest.Proxy is PlaceholderWebProxy))
                    httpWebRequest.Proxy = baseHttpWebRequest.Proxy;

                httpWebRequest.WithHeaders(baseHttpWebRequest.Headers);

                return httpWebRequest as WebRequest;

            }
            else {

                return baseWebRequest;

            }

        }
        protected override WebResponse GetWebResponse(WebRequest request) {

            return webRequestHandler.Send(request, CancellationToken.None);

        }

        protected override void Dispose(bool disposing) {

            if (disposing && !isDisposed) {

                webRequestHandler.Dispose();

                isDisposed = true;

            }

            base.Dispose(disposing);

        }

        // Private members

        private sealed class PlaceholderWebProxy :
            IWebProxy {

            // Public members

            public ICredentials Credentials {
                get => webProxy?.Credentials;
                set => webProxy.Credentials = value;
            }

            public PlaceholderWebProxy(IWebProxy webProxy) {

                this.webProxy = webProxy;

            }

            public Uri GetProxy(Uri destination) {

                if (webProxy is null)
                    return destination;

                return webProxy.GetProxy(destination);

            }
            public bool IsBypassed(Uri host) {

                if (webProxy is null)
                    return true;

                return webProxy.IsBypassed(host);

            }

            // Private members

            private readonly IWebProxy webProxy;

        }

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly WebRequestHandler webRequestHandler = new WebRequestHandler();
        private bool isDisposed = false;

    }

}