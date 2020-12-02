using System.Collections.Generic;
using System.Net;

namespace Gsemac.Net.Extensions {

    public static class WebHeaderCollectionExtensions {

        public static IEnumerable<IHttpHeader> GetHeaders(this WebHeaderCollection headerCollection) {

            foreach (string key in headerCollection.AllKeys)
                yield return new HttpHeader(key, headerCollection[key]);

        }
        public static void CopyTo(this WebHeaderCollection source, WebHeaderCollection desination) {

            foreach (IHttpHeader header in source.GetHeaders())
                desination[header.Name] = header.Value;

        }

    }

}