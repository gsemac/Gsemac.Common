using Gsemac.Polyfills.System.Net;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Gsemac.Net {

    public static class ServicePointManagerUtilities {

        // Public members

        public const SecurityProtocolType Net40SecurityProtocols = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
        public const SecurityProtocolType Net45SecurityProtocols = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        public const SecurityProtocolType Net48SecurityProtocols = SecurityProtocolType.Tls13;

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

        public static void SetSecurityProtocolEnabled(SecurityProtocolType securityProtocol, bool enabled = true) {

            SetSecurityProtocolEnabled((System.Net.SecurityProtocolType)securityProtocol, enabled);

        }
        public static void SetSecurityProtocolEnabled(System.Net.SecurityProtocolType securityProtocol, bool enabled = true) {

            if (enabled)
                System.Net.ServicePointManager.SecurityProtocol |= securityProtocol;
            else
                System.Net.ServicePointManager.SecurityProtocol &= ~securityProtocol;

        }
        public static bool TrySetSecurityProtocolEnabled(SecurityProtocolType securityProtocol, bool enabled = true) {

            return TrySetSecurityProtocolEnabled((System.Net.SecurityProtocolType)securityProtocol, enabled);

        }
        public static bool TrySetSecurityProtocolEnabled(System.Net.SecurityProtocolType securityProtocol, bool enabled = true) {

            try {

                SetSecurityProtocolEnabled(securityProtocol, enabled);

                return true;

            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (NotSupportedException) {

                // A NotSupportedException will be thrown when the enum values are outside the range of System.Net.SecurityProtocolType.
                // This may occur for values added that don't exist in the vanilla framework (such as Tls13 in .NET 4.0).

                return false;

            }
#pragma warning restore CA1031 // Do not catch general exception types

        }
        public static bool IsSecurityProtocolEnabled(SecurityProtocolType securityProtocol) {

            return IsSecurityProtocolEnabled((System.Net.SecurityProtocolType)securityProtocol);

        }
        public static bool IsSecurityProtocolEnabled(System.Net.SecurityProtocolType securityProtocol) {

            return System.Net.ServicePointManager.SecurityProtocol.HasFlag(securityProtocol);

        }

        // Private members

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {

            return true;

        }

    }

}