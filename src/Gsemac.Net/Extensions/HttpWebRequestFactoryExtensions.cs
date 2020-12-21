namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestFactoryExtensions {

        public static IWebClientFactory ToWebClientFactory(this IHttpWebRequestFactory webRequestFactory) {

            return new WebClientFactory(webRequestFactory);

        }

    }

}