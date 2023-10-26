using Gsemac.Net.Properties;
using Gsemac.Polyfills.System.Net;
using System;
using System.Net;

namespace Gsemac.Net.Dns {

    public static class DnsResolver {

        // Public members

        public static IDnsResolver Create(IPEndPoint endpoint) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            return new UdpDnsResolver(endpoint);

        }
        public static IDnsResolver Create(IPAddress endpoint) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            return new UdpDnsResolver(endpoint);

        }
        public static IDnsResolver Create(Uri endpoint) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            string scheme = endpoint.Scheme;

            if (scheme.Equals("http", StringComparison.OrdinalIgnoreCase) || scheme.Equals("https", StringComparison.OrdinalIgnoreCase)) {

                return new HttpDnsResolver(endpoint);

            }
            else if (IPEndPointEx.TryParse(endpoint.Authority, out IPEndPoint ipEndPoint)) {

                if (scheme.Equals("udp", StringComparison.OrdinalIgnoreCase)) {

                    if (ipEndPoint.Port <= 0)
                        return new UdpDnsResolver(ipEndPoint.Address);

                    return new UdpDnsResolver(ipEndPoint);

                }
                else if (scheme.Equals("tcp", StringComparison.OrdinalIgnoreCase)) {

                    if (ipEndPoint.Port <= 0)
                        return new TcpDnsResolver(ipEndPoint.Address);

                    return new TcpDnsResolver(ipEndPoint);

                }

            }

            throw new FormatException(string.Format(ExceptionMessages.InvalidDnsEndpointWithString, endpoint));

        }

        public static IDnsResolver Create(string endpoint) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            if (!TryCreate(endpoint, out IDnsResolver resolver))
                throw new FormatException(string.Format(ExceptionMessages.InvalidDnsEndpointWithString, endpoint));

            return resolver;

        }
        public static bool TryCreate(string endpoint, out IDnsResolver result) {

            result = default;

            if (string.IsNullOrWhiteSpace(endpoint))
                return false;

            if (IPEndPointEx.TryParse(endpoint, out IPEndPoint parsedEndPoint)) {

                if (parsedEndPoint.Port == 0) {

                    result = Create(parsedEndPoint.Address);

                }
                else {

                    result = Create(parsedEndPoint);

                }

            }
            else if (Uri.TryCreate(endpoint, UriKind.Absolute, out Uri parsedUri))
                result = Create(parsedUri);

            return result != default;

        }

    }

}