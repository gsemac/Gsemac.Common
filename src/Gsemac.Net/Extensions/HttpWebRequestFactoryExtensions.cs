using System.Net;

namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestFactoryExtensions {

        public static IWebClientFactory ToWebClientFactory(this IHttpWebRequestFactory webRequestFactory) {

            return new WebClientFactory(webRequestFactory);

        }
        public static IHttpWebRequest Create(this IHttpWebRequestFactory webRequestFactory, string url) {

            return webRequestFactory.Create(new System.Uri(url));

        }
        public static WebClient CreateWebClient(this IHttpWebRequestFactory webRequestFactory) {

            return webRequestFactory.ToWebClientFactory().Create();

        }

    }

}