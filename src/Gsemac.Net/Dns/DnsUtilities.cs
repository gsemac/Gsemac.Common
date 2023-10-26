using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Dns {

    public static class DnsUtilities {

        // Public members

        public static string GetReverseLookupAddress(IPAddress ipAddress) {

            if (ipAddress is null)
                throw new ArgumentNullException(nameof(ipAddress));

            // https://en.wikipedia.org/wiki/Reverse_DNS_lookup#Implementation_details

            byte[] addressBytes = ipAddress.GetAddressBytes();

            if (addressBytes.Length <= 4) {

                // Treat this address as an IPv4 address.

                IEnumerable<string> parts = addressBytes
                    .Reverse()
                    .Select(b => b.ToString(CultureInfo.InvariantCulture));

                return string.Join(".", parts) + ".in-addr.arpa";

            }
            else {

                // Treat this address as an IPv6 address.

                IEnumerable<string> parts = addressBytes
                    .SelectMany(b => new[] { (byte)((b & 0xF0) >> 4), (byte)(b & 0x0F) })
                    .Select(b => b.ToString("x"))
                    .Reverse();

                return string.Join(".", parts) + ".ip6.arpa";

            }

        }

    }

}