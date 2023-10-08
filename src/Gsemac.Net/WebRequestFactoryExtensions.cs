using System;

namespace Gsemac.Net {

    public static class WebRequestFactoryExtensions {

        // Public members

        public static IWebRequest Create(this IWebRequestFactory webRequestFactory, string url) {

            if (webRequestFactory is null)
                throw new ArgumentNullException(nameof(webRequestFactory));

            return webRequestFactory.Create(new Uri(url));

        }

    }

}