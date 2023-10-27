using Gsemac.Net.Dns;
using Gsemac.Net.Http.Extensions;
using Gsemac.Polyfills.System.Net;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Http {

    public sealed class DnsHandler :
        HttpWebRequestHandler {

        // Public members

        public DnsHandler(IDnsClient dnsClient) :
            this(dnsClient, HttpWebRequestFactory.Default) {
        }
        public DnsHandler(IDnsClient dnsClient, IHttpWebRequestFactory httpWebRequestFactory) {

            if (dnsClient is null)
                throw new ArgumentNullException(nameof(dnsClient));

            if (httpWebRequestFactory is null)
                throw new ArgumentNullException(nameof(httpWebRequestFactory));

            this.dnsClient = dnsClient;
            this.httpWebRequestFactory = httpWebRequestFactory;

        }

        // Protected members

        protected override IHttpWebResponse Send(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            // Note that requests using HTTPS may require certificate validation to be disabled to connect by raw IP. 

            string hostname = request.RequestUri.Host;

            if (!IPEndPointEx.TryParse(hostname, out _)) {

                IPAddress ipAddress = dnsClient.GetHostEntry(hostname)
                    .AddressList
                    .FirstOrDefault();

                if (ipAddress is object) {

                    UriBuilder uriBuilder = new UriBuilder(request.RequestUri) {
                        Host = ipAddress.ToString(),
                    };

                    IHttpWebRequest newRequest = httpWebRequestFactory.Create(uriBuilder.Uri);

                    request.CopyTo(newRequest);

                    newRequest.Host = hostname;

                    request = newRequest;

                }

            }

            return base.Send(request, cancellationToken);

        }

        // Private members

        private readonly IDnsClient dnsClient;
        private readonly IHttpWebRequestFactory httpWebRequestFactory;

    }

}