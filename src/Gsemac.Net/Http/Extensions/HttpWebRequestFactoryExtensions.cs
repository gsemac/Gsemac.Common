using System;

namespace Gsemac.Net.Http.Extensions {

    public static class HttpWebRequestFactoryExtensions {

        public static IWebClientFactory ToWebClientFactory(this IHttpWebRequestFactory webRequestFactory) {

            return new WebClientFactory(webRequestFactory);

        }
        public static IHttpWebRequest Create(this IHttpWebRequestFactory webRequestFactory, string url) {

            return webRequestFactory.Create(new Uri(url));

        }
        public static IWebClient CreateWebClient(this IHttpWebRequestFactory webRequestFactory) {

            return webRequestFactory.ToWebClientFactory().Create();

        }

    }

}