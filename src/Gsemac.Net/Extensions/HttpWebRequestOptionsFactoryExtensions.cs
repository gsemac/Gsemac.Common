using System;

namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestOptionsFactoryExtensions {

        public static IHttpWebRequestOptions Create(this IHttpWebRequestOptionsFactory webRequestOptionsFactory, string requestUrl) {

            return webRequestOptionsFactory.Create(new Uri(requestUrl));

        }

    }

}