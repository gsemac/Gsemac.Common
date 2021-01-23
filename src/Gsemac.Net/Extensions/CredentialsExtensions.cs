using System;
using System.Net;

namespace Gsemac.Net.Extensions {

    public static class CredentialsExtensions {

        // Public members

        public static string ToCredentialsString(this ICredentials credentials, Uri uri) {

            return credentials.ToCredentialsString(uri, string.Empty);

        }
        public static string ToCredentialsString(this ICredentials credentials, Uri uri, string authType) {

            if (credentials is null)
                throw new ArgumentNullException(nameof(credentials));

            NetworkCredential networkCredential = credentials?.GetCredential(uri, authType);

            if (networkCredential is null)
                return string.Empty;

            return networkCredential.ToCredentialsString();

        }
        public static string ToCredentialsString(this NetworkCredential credentials) {

            if (credentials is null)
                throw new ArgumentNullException(nameof(credentials));

            return $"{Uri.EscapeDataString(credentials.UserName)}:{Uri.EscapeDataString(credentials.Password)}";

        }

    }

}