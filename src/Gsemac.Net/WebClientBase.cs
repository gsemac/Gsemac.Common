using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
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

            if (webRequestFactory is null)
                throw new ArgumentNullException(nameof(webRequestFactory));

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

            if (webRequestHandler is null)
                throw new ArgumentNullException(nameof(webRequestHandler));

            this.webRequestHandler = webRequestHandler;

        }

        protected override WebRequest GetWebRequest(Uri address) {

            // The only way to get the method is to call base.GetWebRequest() and copy its properties.
            // Note that calling this method also clears most headers from the WebClient.

            IWebProxy proxy = null;

            if (!IsProxySupported(address)) {

                proxy = Proxy;
                Proxy = null;

            }

            WebRequest baseWebRequest = base.GetWebRequest(address);

            // Restore the proxy we removed temporarily. 

            if (proxy is object)
                Proxy = proxy;

            if (baseWebRequest is HttpWebRequest baseHttpWebRequest) {

                IHttpWebRequest httpWebRequest = webRequestFactory.Create(address);

                if (baseHttpWebRequest.Credentials is object)
                    httpWebRequest.Credentials = baseHttpWebRequest.Credentials;

                httpWebRequest.Method = baseHttpWebRequest.Method;

                if (!(baseHttpWebRequest.Proxy is PlaceholderWebProxy))
                    httpWebRequest.Proxy = baseHttpWebRequest.Proxy;

                // If we temporarily removed the proxy, we need to apply it to the request.

                if (proxy is object)
                    httpWebRequest.Proxy = proxy;

                httpWebRequest.WithHeaders(baseHttpWebRequest.Headers);

                return httpWebRequest as WebRequest;

            }
            else {

                return baseWebRequest;

            }

        }
        protected override WebResponse GetWebResponse(WebRequest request) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return webRequestHandler.GetResponse(request, CancellationToken.None);

        }
        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return webRequestHandler.EndGetResponse(request, result);

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

        private bool IsProxySupported(Uri address) {

            if (address is null)
                throw new ArgumentNullException(nameof(address));

            if (Proxy is null)
                return true;

#if NETFRAMEWORK

            // .NET Framework only supports HTTP/HTTPS proxies (SOCKS proxy support was added in .NET 6).
            // "GetWebRequest" will throw if we attempt to create a web request with an unsupported proxy.

            string proxyScheme = Proxy.GetProxy(address).Scheme;

            bool isSupportedScheme = proxyScheme.Equals("http", StringComparison.OrdinalIgnoreCase) ||
                proxyScheme.Equals("https", StringComparison.OrdinalIgnoreCase);

            return isSupportedScheme;

#else

           return true;

#endif

        }

    }

}