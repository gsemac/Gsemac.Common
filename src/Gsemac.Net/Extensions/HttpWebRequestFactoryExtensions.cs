using System.Net;

namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestFactoryExtensions {

        public static IWebClientFactory ToWebClientFactory(this IHttpWebRequestFactory webRequestFactory) {

            return new WebClientFactory(webRequestFactory);

        }
        public static WebClient CreateWebClient(this IHttpWebRequestFactory webRequestFactory) {

            return webRequestFactory.ToWebClientFactory().Create();

        }

    }

}