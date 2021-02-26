using Gsemac.Polyfills.System.Net;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Gsemac.Net {

    public static class ServicePointManagerUtilities {

        // Public members

        public static bool IsCertificateValidationEnabled() {

            return System.Net.ServicePointManager.ServerCertificateValidationCallback != ServerCertificateValidationCallback;

        }
        public static void SetCertificateValidationEnabled(bool enabled) {

            if (enabled) {

                System.Net.ServicePointManager.ServerCertificateValidationCallback = null;

            }
            else {

                System.Net.ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
                System.Net.ServicePointManager.Expect100Continue = true;

            }

        }

        public static bool TrySetSecurityProtocolsEnabled(SecurityProtocolType securityProtocols, bool enabled) {

            return TrySetSecurityProtocolsEnabled((System.Net.SecurityProtocolType)securityProtocols, enabled);

        }
        public static bool TrySetSecurityProtocolsEnabled(System.Net.SecurityProtocolType securityProtocols, bool enabled) {

            try {

                if (enabled)
                    System.Net.ServicePointManager.SecurityProtocol |= securityProtocols;
                else
                    System.Net.ServicePointManager.SecurityProtocol &= ~securityProtocols;

                return true;

            }
            catch (Exception) {

                return false;

            }

        }

        public static bool TrySetNet40SecurityProtocolsEnabled(bool enabled) {

            return TrySetSecurityProtocolsEnabled(SecurityProtocolType.Ssl3, enabled) &&
               TrySetSecurityProtocolsEnabled(SecurityProtocolType.Tls, enabled);

        }
        public static bool TrySetNet45SecurityProtocolsEnabled(bool enabled) {

            return TrySetSecurityProtocolsEnabled(SecurityProtocolType.Tls11, enabled) &&
                TrySetSecurityProtocolsEnabled(SecurityProtocolType.Tls12, enabled);

        }
        public static bool TrySetNet48SecurityProtocolsEnabled(bool enabled) {

            return TrySetSecurityProtocolsEnabled(SecurityProtocolType.Tls13, enabled);

        }

        // Private members

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {

            return true;

        }

    }

}