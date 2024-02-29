using Gsemac.Collections.Extensions;
using System;
using System.Collections.Generic;

namespace Gsemac.Net.Http.Headers {

    public sealed class ContentEncodingComparer :
        IComparer<ContentEncoding> {

        // Public members

        public int Compare(ContentEncoding x, ContentEncoding y) {

            // Encodings are ordered by quality value first, then name.

            int result = y.QualityValue.CompareTo(x.QualityValue);

            if (result == 0)
                result = GetNameOrdering(x).CompareTo(GetNameOrdering(y));

            return result;

        }

        // Private members

        private static int GetNameOrdering(ContentEncoding value) {

            if (value is null)
                return 0;

            if (string.IsNullOrWhiteSpace(value.Name))
                return 0;

            return new[] {
                "gzip",
                "compress",
                "deflate",
                "br",
                "identity",
                "zstd",
                "*",
            }.IndexOf(value.Name, StringComparison.OrdinalIgnoreCase);

        }

    }

}