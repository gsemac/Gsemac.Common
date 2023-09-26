using Gsemac.Net.Http;
using System;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    internal static class CookieUtilities {

        // Public members

        public static string SanitizeValue(string value) {

            // Cookie values may already be URL-encoded, or we might have cookies containing unescaped characters (e.g. commas).
            // The browser stores cookies as they are given, and ideally, we would process them the same way.
            // Unfortunately, CookieContainer will throw if a cookie value contains a comma or semicolon.

            // CookieContainer will allow such cookies if we wrap the value in double-quotes or URL-encode the value.
            // We will use the latter approach, as this is commonly employed by various implementations anyway (e.g. PHP).
            // However, since cookies may already be URL-encoded, we will only do this if they contain illegal characters.

            if (!string.IsNullOrWhiteSpace(value) && value.Any(c => HttpUtilities.GetInvalidCookieChars().Contains(c)))
                value = Uri.EscapeDataString(value);

            return value;

        }
        public static string SanitizePath(string path) {

            // CookieContainer does not allow paths ending with a dot (".").

            if (string.IsNullOrWhiteSpace(path))
                return path;

            return path.TrimEnd('.');

        }


    }

}