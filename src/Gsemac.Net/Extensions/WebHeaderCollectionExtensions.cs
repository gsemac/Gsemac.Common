using Gsemac.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Extensions {

    public static class WebHeaderCollectionExtensions {

        // Public members

        public static IEnumerable<IHttpHeader> GetAll(this WebHeaderCollection headerCollection) {

            foreach (string key in headerCollection.AllKeys)
                yield return new HttpHeader(key, headerCollection[key]);

        }

        public static bool TryGet(this WebHeaderCollection headerCollection, string headerName, out string value) {

            value = default;

            if (headerCollection is null)
                return false;

            try {

                value = headerCollection[headerName];

                return !string.IsNullOrWhiteSpace(value);

            }
            catch (InvalidOperationException) {

                // The WebHeaderCollection will throw an InvalidOperationException if the header type (HttpRequestHeader or HttpResponseHeader) does not match what type the collection allows instances of.
                // What type of header the WebHeaderCollection allows can only be inferred from context and cannot be determined for an arbitrary WebHeaderCollection.

                // This causes problems in some situations such as downloading a file from a local URI using the WebClient class, which will not change the WebHeaderCollection 
                // to one allowing response headers (which it would for a remote URI). The only solution would be checking if the URI was a local URI before trying to read the response headers.

                return false;

            }

        }
        public static bool TryGet(this WebHeaderCollection headerCollection, HttpRequestHeader requestHeader, out string value) {

            value = default;

            if (headerCollection is null)
                return false;

            try {

                value = headerCollection[requestHeader];

                return !string.IsNullOrWhiteSpace(value);

            }
            catch (InvalidOperationException) {

                return false;

            }

        }
        public static bool TryGet(this WebHeaderCollection headerCollection, HttpResponseHeader responseHeader, out string value) {

            value = default;

            if (headerCollection is null)
                return false;

            try {

                value = headerCollection[responseHeader];

                return !string.IsNullOrWhiteSpace(value);

            }
            catch (InvalidOperationException) {

                return false;

            }

        }

        public static bool TrySet(this WebHeaderCollection headerCollection, string headerName, string value) {

            if (headerCollection is null)
                return false;

            try {

                headerCollection[headerName] = value;

                return true;

            }
            catch (Exception) {

                return false;

            }

        }
        public static bool TrySet(this WebHeaderCollection headerCollection, HttpRequestHeader requestHeader, string value) {

            if (headerCollection is null)
                return false;

            try {

                headerCollection[requestHeader] = value;

                return true;

            }
            catch (InvalidOperationException) {

                return false;

            }

        }
        public static bool TrySet(this WebHeaderCollection headerCollection, HttpResponseHeader responseHeader, string value) {

            if (headerCollection is null)
                return false;

            try {

                headerCollection[responseHeader] = value;

                return true;

            }
            catch (Exception) {

                return false;

            }

        }
        public static bool TrySet(this WebHeaderCollection headerCollection, IHttpHeader header) {

            if (headerCollection is null)
                throw new ArgumentNullException(nameof(headerCollection));

            if (header is null)
                throw new ArgumentNullException(nameof(header));

            return TrySet(headerCollection, header.Name, header.Value);

        }
        public static bool TrySet(this WebHeaderCollection headerCollection, IEnumerable<IHttpHeader> headers) {

            if (headerCollection is null)
                throw new ArgumentNullException(nameof(headerCollection));

            if (headers is null)
                throw new ArgumentNullException(nameof(headers));

            return !headers.All(header => headerCollection.TrySet(header));

        }

        public static void CopyTo(this WebHeaderCollection source, WebHeaderCollection destination) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (destination is null)
                throw new ArgumentNullException(nameof(destination));

            foreach (IHttpHeader header in source.GetAll())
                destination[header.Name] = header.Value;

        }
        public static void CopyTo(this NameValueCollection source, WebHeaderCollection destination) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (destination is null)
                throw new ArgumentNullException(nameof(destination));

            foreach (string key in source.AllKeys)
                destination[key] = source[key];

        }

        public static WebHeaderCollection Clone(this WebHeaderCollection source) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            WebHeaderCollection webHeaderCollection = new WebHeaderCollection();

            source.CopyTo(webHeaderCollection);

            return webHeaderCollection;

        }

    }

}