using Gsemac.Collections.Extensions;
using Gsemac.Net.Http.Headers;
using System;
using System.Collections.Generic;

namespace Gsemac.Net.Http {

    internal abstract class HttpHeaderComparerBase :
        IComparer<IHttpHeader> {

        // Public members

        public int Compare(IHttpHeader x, IHttpHeader y) {

            return GetHeaderIndex(x).CompareTo(GetHeaderIndex(y));

        }

        // Protected members

        protected HttpHeaderComparerBase() {

            headerOrdering = new Lazy<string[]>(GetHeaderOrdering);

        }

        protected virtual int GetHeaderIndex(IHttpHeader header) {

            int defaultIndex = headerOrdering.Value?.Length ?? 0;

            if (header is null)
                return defaultIndex;

            int headerIndex = headerOrdering.Value.IndexOf(header.Name.ToLowerInvariant());

            if (headerIndex < 0)
                return defaultIndex;

            return headerIndex;

        }
        protected abstract string[] GetHeaderOrdering();

        // Private members

        private readonly Lazy<string[]> headerOrdering;

    }

}