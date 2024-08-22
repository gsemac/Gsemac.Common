using System;

namespace Gsemac.Net {

    public static class PublicSuffixListExtensions {

        // Public members

        public static string GetSuffix(this IPublicSuffixList publicSuffixList, string hostname) {

            if (publicSuffixList is null)
                throw new ArgumentNullException(nameof(publicSuffixList));

            if (string.IsNullOrWhiteSpace(hostname))
                return string.Empty;

            string[] hostnameParts = hostname.Split('.');

            int startIndex = 0;

            while (true) {



            }

        }

    }

}