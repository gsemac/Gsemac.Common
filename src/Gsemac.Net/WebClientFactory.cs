namespace Gsemac.Net {

    public class WebClientFactory :
        IWebClientFactory {

        // Public members

        public WebClientFactory() {

            webRequestFactory = null;

        }
        public WebClientFactory(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }

        public System.Net.WebClient CreateWebClient() {

            return webRequestFactory is null ?
                new System.Net.WebClient() :
                new WebClient(webRequestFactory);

        }

        // Private members

        private class WebClient :
            WebClientBase {

            // Public members

            public WebClient(IHttpWebRequestFactory webRequestFactory) :
                base(webRequestFactory) {
            }

        }

        private readonly IHttpWebRequestFactory webRequestFactory;

    }

}