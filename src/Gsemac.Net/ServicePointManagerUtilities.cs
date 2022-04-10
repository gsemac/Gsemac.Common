using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Gsemac.Net {

    public static class ServicePointManagerUtilities {

        // Public members

        public const SecurityProtocolTypeEx Net40SecurityProtocols = SecurityProtocolTypeEx.Ssl3 | SecurityProtocolTypeEx.Tls;
        public const SecurityProtocolTypeEx Net45SecurityProtocols = SecurityProtocolTypeEx.Tls11 | SecurityProtocolTypeEx.Tls12;
        public const SecurityProtocolTypeEx Net48SecurityProtocols = SecurityProtocolTypeEx.Tls13;

        public static bool IsCertificateValidationEnabled() {

            return ServicePointManager.ServerCertificateValidationCallback != ServerCertificateValidationCallback;

        }
        public static void SetCertificateValidationEnabled(bool enabled) {

            if (enabled) {

                ServicePointManager.ServerCertificateValidationCallback = null;

            }
            else {

                ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
                ServicePointManager.Expect100Continue = true;

            }

        }

        public static void SetSecurityProtocolEnabled(SecurityProtocolTypeEx securityProtocol, bool enabled = true) {

            SetSecurityProtocolEnabled((SecurityProtocolType)securityProtocol, enabled);

        }
        public static void SetSecurityProtocolEnabled(SecurityProtocolType securityProtocol, bool enabled = true) {

            if (enabled)
                ServicePointManager.SecurityProtocol |= securityProtocol;
            else
                ServicePointManager.SecurityProtocol &= ~securityProtocol;

        }
        public static bool TrySetSecurityProtocolEnabled(SecurityProtocolTypeEx securityProtocol, bool enabled = true) {

            return TrySetSecurityProtocolEnabled((SecurityProtocolType)securityProtocol, enabled);

        }
        public static bool TrySetSecurityProtocolEnabled(SecurityProtocolType securityProtocol, bool enabled = true) {

            try {

                SetSecurityProtocolEnabled(securityProtocol, enabled);

                return true;

            }
            catch (NotSupportedException) {

                // A NotSupportedException will be thrown when the enum values are outside the range of System.Net.SecurityProtocolType.
                // This may occur for values added that don't exist in the vanilla framework (such as Tls13 in .NET 4.0).

                return false;

            }

        }
        public static bool IsSecurityProtocolEnabled(SecurityProtocolTypeEx securityProtocol) {

            return IsSecurityProtocolEnabled((SecurityProtocolType)securityProtocol);

        }
        public static bool IsSecurityProtocolEnabled(SecurityProtocolType securityProtocol) {

            return ServicePointManager.SecurityProtocol.HasFlag(securityProtocol);

        }

        // Private members

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {

            return true;

        }

    }

}