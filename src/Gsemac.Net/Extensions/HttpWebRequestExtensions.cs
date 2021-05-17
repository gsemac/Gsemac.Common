using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestExtensions {

        // Public members

        public static void SetHeaderValue(this IHttpWebRequest httpWebRequest, string headerName, string value) {

            switch (headerName.ToLowerInvariant()) {

                case "accept":
                    httpWebRequest.Accept = value;
                    break;

                case "content-type":
                    httpWebRequest.ContentType = value;
                    break;

                case "range": {

                        var range = ParseRangeHeader(value);

                        httpWebRequest.AddRange(range.Item1, range.Item2);

                    }
                    break;

                case "referer":
                    httpWebRequest.Referer = value;
                    break;

                case "user-agent":
                    httpWebRequest.UserAgent = value;
                    break;

                default:
                    httpWebRequest.Headers.Set(headerName, value);
                    break;

            }

        }
        public static void SetHeaderValue(this HttpWebRequest httpWebRequest, string headerName, string value) {

            SetHeaderValue(new HttpWebRequestWrapper(httpWebRequest), headerName, value);

        }

        public static IHttpWebRequest WithOptions(this IHttpWebRequest httpWebRequest, IHttpWebRequestOptions options, bool copyIfNull = true) {

            if (copyIfNull || !string.IsNullOrWhiteSpace(options.Accept))
                httpWebRequest.Accept = options.Accept;

            if (copyIfNull || !string.IsNullOrWhiteSpace(options.AcceptLanguage))
                httpWebRequest.Headers[HttpRequestHeader.AcceptLanguage] = options.AcceptLanguage;

            if (copyIfNull || options.AllowAutoRedirect.HasValue)
                httpWebRequest.AllowAutoRedirect = options.AllowAutoRedirect ?? httpWebRequest.AllowAutoRedirect;

            httpWebRequest.AutomaticDecompression = options.AutomaticDecompression;

            if (copyIfNull || options.Cookies is object)
                httpWebRequest.CookieContainer = options.Cookies;

            if (copyIfNull || options.Credentials is object)
                httpWebRequest.Credentials = options.Credentials;

            if (copyIfNull || options.Proxy is object)
                httpWebRequest.Proxy = options.Proxy;

            if (copyIfNull || !string.IsNullOrWhiteSpace(options.UserAgent))
                httpWebRequest.UserAgent = options.UserAgent;

            return httpWebRequest;

        }
        public static HttpWebRequest WithOptions(this HttpWebRequest httpWebRequest, IHttpWebRequestOptions options, bool copyIfNull = true) {

            WithOptions(new HttpWebRequestWrapper(httpWebRequest), options, copyIfNull);

            return httpWebRequest;

        }
        public static IHttpWebRequest WithHeaders(this IHttpWebRequest httpWebRequest, WebHeaderCollection headers) {

            foreach (IHttpHeader header in headers.GetHeaders())
                httpWebRequest.SetHeaderValue(header.Name, header.Value);

            return httpWebRequest;

        }
        public static HttpWebRequest WithHeaders(this HttpWebRequest httpWebRequest, WebHeaderCollection headers) {

            foreach (IHttpHeader header in headers.GetHeaders())
                httpWebRequest.SetHeaderValue(header.Name, header.Value);

            return httpWebRequest;

        }

        // Private members

        private static Tuple<long, long> ParseRangeHeader(string headerValue) {

            Match match = Regex.Match(headerValue, @"(\d+)-(\d+)");

            if (!match.Success)
                throw new ArgumentNullException(nameof(headerValue));

            long first = long.Parse(match.Groups[1].Value);
            long second = long.Parse(match.Groups[2].Value);

            return new Tuple<long, long>(first, second);

        }

    }

}