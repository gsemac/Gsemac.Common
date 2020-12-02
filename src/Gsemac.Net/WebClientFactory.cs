namespace Gsemac.Net {

    public class WebClientFactory :
        IWebClientFactory {

        // Public members

        public IHttpWebRequestOptions Options {
            get => webRequestFactory.Options;
            set => webRequestFactory.Options = value;
        }

        public WebClientFactory(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }

        public System.Net.WebClient CreateWebClient() {

            return new WebClient(webRequestFactory);

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