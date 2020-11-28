using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gsemac.Net.Extensions {

    public static class WebProxyExtensions {

        public static string ToProxyString(this IWebProxy proxy, Uri destination) {

            if (proxy is null)
                throw new ArgumentNullException(nameof(proxy));

            if (destination is null)
                throw new ArgumentNullException(nameof(destination));

            Uri proxyUri = proxy.GetProxy(destination);
            NetworkCredential credentials = proxy.Credentials?.GetCredential(destination, "Basic");

            StringBuilder sb = new StringBuilder();

            sb.Append(proxyUri.GetLeftPart(UriPartial.Scheme));

            if (!(credentials is null)) {

                sb.Append(credentials.ToCredentialString());
                sb.Append("@");

            }

            sb.Append(proxyUri.Host);
            sb.Append(string.Format(":{0}", proxyUri.Port));

            return sb.ToString();

        }

    }

}
