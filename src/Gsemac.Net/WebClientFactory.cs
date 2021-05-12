namespace Gsemac.Net {

    public class WebClientFactory :
        IWebClientFactory {

        // Public members

        public static WebClientFactory Default => new WebClientFactory();

        public WebClientFactory() :
            this(HttpWebRequestFactory.Default) {
        }
        public WebClientFactory(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }

        public IWebClient Create() {

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