using System;
using System.Net;
using System.Text;

namespace Gsemac.Net.Extensions {

    public static class WebProxyExtensions {

        // Public members

        public static bool IsEmpty(this IWebProxy proxy) {

            return proxy.IsEmpty(PlaceholderUri);

        }
        public static bool IsEmpty(this IWebProxy proxy, Uri destination) {

            if (proxy is null)
                return true;

            return proxy.IsBypassed(destination) ||
                proxy.GetProxy(destination).Equals(destination);

        }

        public static string ToProxyString(this IWebProxy proxy) {

            return proxy.ToProxyString(PlaceholderUri);

        }
        public static string ToProxyString(this IWebProxy proxy, Uri destination) {

            if (proxy is null)
                throw new ArgumentNullException(nameof(proxy));

            if (destination is null)
                throw new ArgumentNullException(nameof(destination));

            Uri proxyUri = proxy.GetProxy(destination);
            NetworkCredential credentials = proxy.Credentials?.GetCredential(destination, string.Empty);

            // If this URI is not proxied, return an empty string.

            if (proxyUri.Equals(destination))
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append(proxyUri.GetLeftPart(UriPartial.Scheme));

            if (!(credentials is null)) {

                sb.Append(credentials.ToCredentialsString());
                sb.Append("@");

            }

            sb.Append(proxyUri.Host);
            sb.Append(string.Format(":{0}", proxyUri.Port < 0 ? 1080 : proxyUri.Port));

            return sb.ToString();

        }

        // Private members

        private static Uri PlaceholderUri => new Uri("http://example.com");

    }

}