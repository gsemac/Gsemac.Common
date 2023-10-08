using System;

namespace Gsemac.Net.Http {

    public static class HttpWebRequestFactoryExtensions {

        // Public members

        public static IHttpWebRequest Create(this IHttpWebRequestFactory webRequestFactory, string url) {

            if (webRequestFactory is null)
                throw new ArgumentNullException(nameof(webRequestFactory));

            return webRequestFactory.Create(new Uri(url));

        }

    }

}