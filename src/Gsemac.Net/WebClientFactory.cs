using Gsemac.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net {

    public class WebClientFactory :
        IWebClientFactory {

        // Public members

        public static WebClientFactory Default => new WebClientFactory();

        public WebClientFactory() :
            this(HttpWebRequestFactory.Default) {
        }
        public WebClientFactory(IHttpWebRequestFactory webRequestFactory) {

            if (webRequestFactory is null)
                throw new ArgumentNullException(nameof(webRequestFactory));

            this.webRequestFactory = webRequestFactory;

        }
        public WebClientFactory(WebRequestHandler webRequestHandler) :
            this(HttpWebRequestFactory.Default, webRequestHandler) {
        }
        public WebClientFactory(IHttpWebRequestFactory webRequestFactory, WebRequestHandler webRequestHandler) :
            this(webRequestFactory) {

            if (webRequestHandler is null)
                throw new ArgumentNullException(nameof(webRequestHandler));

            this.webRequestHandler = webRequestHandler;

        }

        public IWebClient Create() {

            return new WebClient(webRequestFactory, webRequestHandler);

        }

        public static WebRequestHandler CreatePipeline(WebRequestHandler innerHandler, IEnumerable<DelegatingWebRequestHandler> handlers) {

            if (innerHandler is null)
                throw new ArgumentNullException(nameof(innerHandler));

            if (handlers is null)
                throw new ArgumentNullException(nameof(handlers));

            if (!handlers.Any())
                return innerHandler;

            WebRequestHandler lastHandler = innerHandler;

            foreach (DelegatingWebRequestHandler handler in handlers.Reverse()) {

                handler.InnerHandler = lastHandler;

                lastHandler = handler;

            }

            return lastHandler;

        }

        // Private members

        private class WebClient :
            WebClientBase {

            // Public members

            public WebClient(IHttpWebRequestFactory webRequestFactory, WebRequestHandler webRequestHandler) :
                base(webRequestFactory, webRequestHandler) {
            }

        }

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly WebRequestHandler webRequestHandler = new WebRequestHandler();

    }

}