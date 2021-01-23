using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestExtensions {

        // Public members

        public static void SetHeaderValue(this HttpWebRequest webRequest, string headerName, string headerValue) {

            SetHeaderValue(new HttpWebRequestWrapper(webRequest), headerName, headerValue);

        }
        public static void SetHeaderValue(this IHttpWebRequest webRequest, string headerName, string headerValue) {

            switch (headerName.ToLowerInvariant()) {

                case "accept":
                    webRequest.Accept = headerValue;
                    break;

                case "content-type":
                    webRequest.ContentType = headerValue;
                    break;

                case "range": {

                        var range = ParseRangeHeader(headerValue);

                        webRequest.AddRange(range.Item1, range.Item2);

                    }
                    break;

                case "referer":
                    webRequest.Referer = headerValue;
                    break;

                case "user-agent":
                    webRequest.UserAgent = headerValue;
                    break;

                default:
                    webRequest.Headers.Set(headerName, headerValue);
                    break;

            }

        }

        public static IHttpWebRequest WithOptions(this IHttpWebRequest webRequest, IHttpWebRequestOptions options, bool copyIfNull = true) {

            if (copyIfNull || !string.IsNullOrWhiteSpace(options.Accept))
                webRequest.Accept = options.Accept;

            if (copyIfNull || !string.IsNullOrWhiteSpace(options.AcceptLanguage))
                webRequest.Headers[HttpRequestHeader.AcceptLanguage] = options.AcceptLanguage;

            webRequest.AutomaticDecompression = options.AutomaticDecompression;

            if (copyIfNull || options.Cookies is object)
                webRequest.CookieContainer = options.Cookies;

            if (copyIfNull || options.Credentials is object)
                webRequest.Credentials = options.Credentials;

            if (copyIfNull || options.Proxy is object)
                webRequest.Proxy = options.Proxy;

            if (copyIfNull || !string.IsNullOrWhiteSpace(options.UserAgent))
                webRequest.UserAgent = options.UserAgent;

            return webRequest;

        }
        public static IHttpWebRequest WithHeaders(this IHttpWebRequest webRequest, WebHeaderCollection headers) {

            foreach (IHttpHeader header in headers.GetHeaders())
                webRequest.SetHeaderValue(header.Name, header.Value);

            return webRequest;

        }
        public static HttpWebRequest WithHeaders(this HttpWebRequest webRequest, WebHeaderCollection headers) {

            foreach (IHttpHeader header in headers.GetHeaders())
                webRequest.SetHeaderValue(header.Name, header.Value);

            return webRequest;

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