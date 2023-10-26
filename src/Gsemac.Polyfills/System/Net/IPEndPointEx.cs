using System;
using System.Globalization;
using System.Net;

namespace Gsemac.Polyfills.System.Net {

    /// <inheritdoc cref="IPEndPoint"/>
    public static class IPEndPointEx {

        // Public members

        public static IPEndPoint Parse(string s) {

            if (s is null)
                throw new ArgumentNullException(nameof(s));

            if (!TryParse(s, out IPEndPoint result))
                throw new FormatException($"'{s}' is not a valid IP endpoint.");

            return result;

        } // .NET Core 3.0 and later
        public static bool TryParse(string s, out IPEndPoint result) {

            result = default;

            if (string.IsNullOrWhiteSpace(s))
                return false;

            s = s.Trim();

            string ipAddressStr = s;

            if (s.StartsWith("[")) {

                // We have an IPv6 address enclosed in braces.
                // e.g. "[2001:db8:85a3:8d3:1319:8a2e:370:7348]:443"
                // https://en.wikipedia.org/wiki/IPv6_address#Literal_IPv6_addresses_in_network_resource_identifiers

                int closingBraceIndex = s.IndexOf(']');

                if (closingBraceIndex < 0)
                    return false;

                ipAddressStr = s.Substring(1, closingBraceIndex - 1);

                // Strip the IP address from the string to parse the port number later.

                s = s.Substring(closingBraceIndex + 1);

            }
            else if (s.Contains(".")) {

                // We have an IPv4 address.
                // e.g. 192.168.1.1
                // We can safely assume everything up to the colon is part of the IP address.

                int colonIndex = s.IndexOf(':');

                if (colonIndex >= 0) {

                    ipAddressStr = s.Substring(0, colonIndex);

                    // Strip the IP address from the string to parse the port number later.

                    s = s.Substring(colonIndex);

                }

            }

            if (!IPAddress.TryParse(ipAddressStr, out IPAddress ipAddress))
                return false;

            int port = 0;

            if (s.StartsWith(":"))
                int.TryParse(s.Substring(1), NumberStyles.Integer, CultureInfo.InvariantCulture, out port);

            result = new IPEndPoint(ipAddress, port);

            return result != default;

        } // .NET Core 3.0 and later

    }

}