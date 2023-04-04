using Gsemac.Polyfills.System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Http {

    public static class HttpUtilities {

        // Public members

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

        public static string GetHeaderName(HttpRequestHeader header) {

            return GetFirstHeaderName(new WebHeaderCollection {
                { header, "x" }
            });

        }
        public static string GetHeaderName(HttpResponseHeader header) {

            return GetFirstHeaderName(new WebHeaderCollection {
                { header, "x" }
            });

        }

        public static string GetStatusDescription(HttpStatusCode statusCode) {

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Status

            switch (statusCode) {

                // Informational responses (100–199)

                case HttpStatusCode.Continue:
                    return "Continue";

                case HttpStatusCode.SwitchingProtocols:
                    return "Switching Protocols";

                case (HttpStatusCode)102:
                    return "Processing";

                case (HttpStatusCode)103:
                    return "Early Hints";

                // Successful responses (200–299)

                case HttpStatusCode.OK:
                    return "OK";

                case HttpStatusCode.Created:
                    return "Created";

                case HttpStatusCode.Accepted:
                    return "Accepted";

                case HttpStatusCode.NonAuthoritativeInformation:
                    return "Non-Authoritative Information";

                case HttpStatusCode.NoContent:
                    return "No Content";

                case HttpStatusCode.ResetContent:
                    return "Reset Content";

                case HttpStatusCode.PartialContent:
                    return "Partial Content";

                case (HttpStatusCode)207:
                    return "Multi-Status";

                case (HttpStatusCode)208:
                    return "Already Reported";

                case (HttpStatusCode)226:
                    return "IM Used";

                // Redirection messages (300–399)

                case HttpStatusCode.MultipleChoices:
                    return "Multiple Choices";

                case HttpStatusCode.MovedPermanently:
                    return "Moved Permanently";

                case HttpStatusCode.Found:
                    return "Found";

                case HttpStatusCode.SeeOther:
                    return "See Other";

                case HttpStatusCode.NotModified:
                    return "Not Modified";

                case HttpStatusCode.UseProxy:
                    return "Use Proxy";

                case HttpStatusCode.Unused:
                    return "unused";

                case HttpStatusCode.TemporaryRedirect:
                    return "Temporary Redirect";

                case (HttpStatusCode)308:
                    return "Permanent Redirect";

                // Client error responses (400–499)

                case HttpStatusCode.BadRequest:
                    return "Bad Request";

                case HttpStatusCode.Unauthorized:
                    return "Unauthorized";

                case HttpStatusCode.PaymentRequired:
                    return "Payment Required";

                case HttpStatusCode.Forbidden:
                    return "Forbidden";

                case HttpStatusCode.NotFound:
                    return "Not Found";

                case HttpStatusCode.MethodNotAllowed:
                    return "Method Not Allowed";

                case HttpStatusCode.NotAcceptable:
                    return "Not Acceptable";

                case HttpStatusCode.ProxyAuthenticationRequired:
                    return "Proxy Authentication Required";

                case HttpStatusCode.RequestTimeout:
                    return "Request Timeout";

                case HttpStatusCode.Conflict:
                    return "Conflict";

                case HttpStatusCode.Gone:
                    return "Gone";

                case HttpStatusCode.LengthRequired:
                    return "Length Required";

                case HttpStatusCode.PreconditionFailed:
                    return "Precondition Failed";

                case HttpStatusCode.RequestEntityTooLarge:
                    return "Payload Too Large";

                case HttpStatusCode.RequestUriTooLong:
                    return "URI Too Long";

                case HttpStatusCode.UnsupportedMediaType:
                    return "Unsupported Media Type";

                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    return "Range Not Satisfiable";

                case HttpStatusCode.ExpectationFailed:
                    return "Expectation Failed";

                case (HttpStatusCode)418:
                    return "I'm a teapot";

                case (HttpStatusCode)421:
                    return "Misdirected Request";

                case (HttpStatusCode)422:
                    return "Unprocessable Entity";

                case (HttpStatusCode)423:
                    return "Locked";

                case (HttpStatusCode)424:
                    return "Failed Dependency";

                case (HttpStatusCode)425:
                    return "Too Early";

                case (HttpStatusCode)426:
                    return "Upgrade Required";

                case (HttpStatusCode)428:
                    return "Precondition Required";

                case (HttpStatusCode)429:
                    return "Too Many Requests";

                case (HttpStatusCode)431:
                    return "Request Header Fields Too Large";

                case (HttpStatusCode)451:
                    return "Unavailable For Legal Reasons";

                // Server error responses (500–599)

                case HttpStatusCode.InternalServerError:
                    return "Internal Server Error";

                case HttpStatusCode.NotImplemented:
                    return "Not Implemented";

                case HttpStatusCode.BadGateway:
                    return "Bad Gateway";

                case HttpStatusCode.ServiceUnavailable:
                    return "Service Unavailable";

                case HttpStatusCode.GatewayTimeout:
                    return "Gateway Timeout";

                case HttpStatusCode.HttpVersionNotSupported:
                    return "HTTP Version Not Supported";

                case (HttpStatusCode)506:
                    return "Variant Also Negotiates";

                case (HttpStatusCode)507:
                    return "Insufficient Storage";

                case (HttpStatusCode)508:
                    return "Loop Detected";

                case (HttpStatusCode)510:
                    return "Not Extended";

                case (HttpStatusCode)511:
                    return "Network Authentication Required";

                default:
                    return string.Empty;

            }

        }

        public static string GetAcceptEncodingString(DecompressionMethods decompressionMethods) {

            return GetAcceptEncodingString((DecompressionMethodsEx)decompressionMethods);

        }

        // Private members

        private static string GetFirstHeaderName(WebHeaderCollection headers) {

            return headers.AllKeys.Single();

        }
        private static string GetAcceptEncodingString(DecompressionMethodsEx decompressionMethods) {

            List<string> methodStrings = new List<string>();

            if (decompressionMethods.HasFlag(DecompressionMethodsEx.All) || decompressionMethods.HasFlag(DecompressionMethodsEx.GZip))
                methodStrings.Add("gzip");

            if (decompressionMethods.HasFlag(DecompressionMethodsEx.All) || decompressionMethods.HasFlag(DecompressionMethodsEx.Deflate))
                methodStrings.Add("deflate");

            if (decompressionMethods.HasFlag(DecompressionMethodsEx.All) || decompressionMethods.HasFlag(DecompressionMethodsEx.Brotli))
                methodStrings.Add("br");

            return string.Join(", ", methodStrings);

        }

    }

}