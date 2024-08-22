using System;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Net {

    internal sealed class PublicSuffixList :
        IPublicSuffixList {

        // Public members

        public PublicSuffixList(IEnumerable<string> suffixList) {

            if (suffixList is null)
                throw new ArgumentNullException(nameof(suffixList));

            suffixLookup = new HashSet<string>(suffixList, StringComparer.OrdinalIgnoreCase);

        }

        public bool Contains(string suffix) {

            if (string.IsNullOrWhiteSpace(suffix))
                return false;

            return suffixLookup.Contains(suffix);

        }

        public IEnumerator<string> GetEnumerator() {
            return suffixLookup.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return suffixLookup.GetEnumerator();
        }

        // Private members

        private readonly HashSet<string> suffixLookup;

    }

}