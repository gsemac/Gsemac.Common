using System.Collections.Generic;

namespace Gsemac.Net.Extensions {

    public static class HttpHeadersReaderExtensions {

        public static IEnumerable<IHttpHeader> ReadHeaders(this IHttpHeadersReader reader) {

            while (reader.ReadNextHeader(out IHttpHeader header))
                yield return header;

        }

    }

}