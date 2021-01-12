using System.Collections.Generic;
using System.Collections.Specialized;
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
        public static void CopyTo(this NameValueCollection source, WebHeaderCollection desination) {

            foreach (string key in source.AllKeys)
                desination[key] = source[key];

        }

    }

}