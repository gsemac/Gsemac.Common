using System;

namespace Gsemac.Net.Extensions {

    public static class UrlBuilderExtensions {

        // Public members

        public static Uri ToUri(this IUrlBuilder urlBuilder) {

            if (urlBuilder is null)
                throw new ArgumentNullException(nameof(urlBuilder));

            return new Uri(urlBuilder.ToString());

        }

    }

}