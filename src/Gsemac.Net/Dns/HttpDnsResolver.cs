using Gsemac.Net.Http;
using Gsemac.Net.Properties;
using Gsemac.Text.Codecs;
using System;
using System.IO;
using System.Net;

namespace Gsemac.Net.Dns {

    public sealed class HttpDnsResolver :
        IDnsResolver {

        // Public members

        public string Method {
            get => method;
            set {

                if (!value.Equals("GET", StringComparison.OrdinalIgnoreCase) && !value.Equals("POST", StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException(ExceptionMessages.DnsOverHttpMustUseGetOrPost, nameof(value));

                method = value;

            }
        }
        public TimeSpan Timeout { get; set; } = DefaultTimeout;

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
            Timeout = timeout;
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

            // TODO: Implement JSON-based requests.
            // This is important because some resolvers only support GET with JSON.

            IHttpWebRequest request = CreateRequest(message);

            try {

                if (method.Equals("POST", StringComparison.OrdinalIgnoreCase)) {

                    using (MemoryStream requestStream = new MemoryStream()) {

                        serializer.Serialize(requestStream, message);

                        byte[] requestBytes = requestStream.ToArray();

                        request.ContentLength = requestBytes.Length;

                        using (Stream stream = request.GetRequestStream())
                            stream.Write(requestBytes, 0, requestBytes.Length);

                    }

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
            catch (WebException ex) {

                if (ex.Status == WebExceptionStatus.Timeout)
                    throw new TimeoutException(ExceptionMessages.DnsRequestTimedOut, ex);

                throw;

            }

        }

        // Private members

        private static readonly string DefaultMethod = "POST";
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

        private readonly IDnsMessageSerializer serializer;
        private readonly Uri endpoint;
        private readonly IHttpWebRequestFactory httpWebRequestFactory;
        private string method = DefaultMethod;

        private IHttpWebRequest CreateRequest(IDnsMessage message) {

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            Uri finalEndpoint = endpoint;

            if (method.Equals("GET", StringComparison.OrdinalIgnoreCase)) {

                string base64Message;

                using (MemoryStream stream = new MemoryStream()) {

                    serializer.Serialize(stream, message);

                    base64Message = Base64Url.EncodeString(stream.ToArray());

                }

                finalEndpoint = new Uri(finalEndpoint.AbsoluteUri + $"?dns={base64Message}");

            }

            IHttpWebRequest request = httpWebRequestFactory.Create(finalEndpoint);

            request.Accept = "application/dns-message";
            request.ContentType = "application/dns-message";
            request.Method = method;
            request.Timeout = (int)Timeout.TotalMilliseconds;

            return request;

        }

    }

}