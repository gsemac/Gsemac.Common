using System;

namespace Gsemac.Net.Http.Extensions {

    public static class HttpWebRequestOptionsFactoryExtensions {

        // Public members

        public static IHttpWebRequestOptions Create(this IHttpWebRequestOptionsFactory webRequestOptionsFactory, string requestUrl) {

            return webRequestOptionsFactory.Create(new Uri(requestUrl));

        }

    }

}