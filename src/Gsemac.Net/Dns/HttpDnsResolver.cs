using Gsemac.Net.Http;
using System;
using System.IO;
using System.Net;

namespace Gsemac.Net.Dns {

    public sealed class HttpDnsResolver :
        IDnsResolver {

        // Public members

        public HttpDnsResolver(Uri endpoint) :
            this(endpoint, DefaultTimeout) {
        }
        public HttpDnsResolver(Uri endpoint, TimeSpan timeout) :
            this(endpoint, HttpWebRequestFactory.Default, timeout) {
        }
        public HttpDnsResolver(Uri endpoint, IHttpWebRequestFactory httpWebRequestFactory) :
            this(endpoint, httpWebRequestFactory, DefaultTimeout) {
        }
        public HttpDnsResolver(Uri endpoint, IHttpWebRequestFactory httpWebRequestFactory, TimeSpan timeout) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            if (httpWebRequestFactory is null)
                throw new ArgumentNullException(nameof(httpWebRequestFactory));

            this.endpoint = endpoint;
            this.timeout = timeout;
            this.httpWebRequestFactory = httpWebRequestFactory;

            serializer = new DnsMessageSerializer();

        }

        public IDnsMessage Resolve(IDnsMessage message) {

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            // We can make a request over GET or POST, using JSON or DNS request data.

            // See the following pages for more detailed information:
            // https://developers.cloudflare.com/1.1.1.1/encryption/dns-over-https/make-api-requests/
            // https://developers.google.com/speed/public-dns/docs/doh

            // TODO: Implement GET and JSON-based requests.
            // TODO: Apply timeout to requests.

            IHttpWebRequest request = httpWebRequestFactory.Create(endpoint);

            request.Accept = "application/dns-message";
            request.ContentType = "application/dns-message";
            request.Method = "POST";
            request.Timeout = (int)timeout.TotalMilliseconds;

            using (MemoryStream requestStream = new MemoryStream()) {

                serializer.Serialize(requestStream, message);

                byte[] requestBytes = requestStream.ToArray();

                request.ContentLength = requestBytes.Length;

                using (Stream stream = request.GetRequestStream())
                    stream.Write(requestBytes, 0, requestBytes.Length);

            }

            using (WebResponse response = request.GetResponse()) {

                int responseLength = (int)response.ContentLength;

                byte[] responseBytes = new byte[responseLength];

                using (Stream stream = response.GetResponseStream()) {

                    stream.Read(responseBytes, 0, responseBytes.Length);

                    using (MemoryStream responseStream = new MemoryStream(responseBytes))
                        return serializer.Deserialize(responseStream);

                }

            }

        }

        // Private members

        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        private readonly TimeSpan timeout;
        private readonly IDnsMessageSerializer serializer;
        private readonly Uri endpoint;
        private readonly IHttpWebRequestFactory httpWebRequestFactory;

    }

}