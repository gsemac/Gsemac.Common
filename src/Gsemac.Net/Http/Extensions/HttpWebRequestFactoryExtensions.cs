namespace Gsemac.Net.Http.Extensions {

    public static class HttpWebRequestFactoryExtensions {

        // Public members

        public static IWebClientFactory ToWebClientFactory(this IHttpWebRequestFactory webRequestFactory) {

            return new WebClientFactory(webRequestFactory);

        }
        public static IWebClient CreateWebClient(this IHttpWebRequestFactory webRequestFactory) {

            return webRequestFactory.ToWebClientFactory().Create();

        }

    }

}