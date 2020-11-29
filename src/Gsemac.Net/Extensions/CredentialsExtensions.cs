using System;
using System.Net;

namespace Gsemac.Net.Extensions {

    public static class CredentialsExtensions {

        public static string ToCredentialString(this ICredentials credentials, Uri uri) {

            return credentials.ToCredentialString(uri, string.Empty);

        }
        public static string ToCredentialString(this ICredentials credentials, Uri uri, string authType) {

            if (credentials is null)
                throw new ArgumentNullException(nameof(credentials));

            NetworkCredential NetworkCredential = credentials?.GetCredential(uri, authType);

            if (NetworkCredential is null)
                return string.Empty;

            return NetworkCredential.ToCredentialString();

        }
        public static string ToCredentialString(this NetworkCredential credentials) {

            if (credentials is null)
                throw new ArgumentNullException(nameof(credentials));

            return $"{Uri.EscapeDataString(credentials.UserName)}:{Uri.EscapeDataString(credentials.Password)}";

        }

    }

}