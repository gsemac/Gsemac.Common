using System.Net;

namespace Gsemac.Net.Http {

    public static class HttpUtilities {

        public static char[] GetInvalidCookieChars() {

            // There is a detailed discussion of invalid cookie characters here:
            // https://stackoverflow.com/a/1969339/5383169 (bobince)

            // Despite RFC 6265 being the most recent standard, in the "real world" we're still using Netscape's cookie_spec.
            // http://curl.haxx.se/rfc/cookie_spec.html
            // This specification dictates that the only invalid characters are "semi-colon, comma and white space".

            // Even so, browsers will allow surrounding whitespace (trimming it).
            // It's also not unheard of to encounter cookies that ignore this specification altogether and still contain commas.
            // Therefore, a robust cookie container implmentation should be prepared for anything.

            // That said, this method is useful for sanitizing cookies before adding them to a Net.CookieContainer, which will throw on invalid characters.

            return new[] { ',', ';' };

        }

        public static bool IsRedirectStatusCode(HttpStatusCode statusCode) {

            switch (statusCode) {

                case HttpStatusCode.Ambiguous:
                case HttpStatusCode.Moved:
                case HttpStatusCode.Redirect:
                case HttpStatusCode.RedirectMethod:
                case HttpStatusCode.RedirectKeepVerb:
                case (HttpStatusCode)308: // PermanentRedirect, doesn't exist in .NET 4.0
                    return true;

                default:
                    return false;

            }

        }
        public static bool IsSuccessStatusCode(HttpStatusCode statusCode) {

            return !((int)statusCode >= 400 && (int)statusCode < 600);

        }

    }

}