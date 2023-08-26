using Gsemac.Net.Extensions;
using Gsemac.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Http.Extensions
{

    public static class HttpWebRequestExtensions {

        // Public members

        public static void SetHeader(this IHttpWebRequest httpWebRequest, string headerName, string value) {

            // A list of restricted headers can be found here: https://stackoverflow.com/a/4752359/5383169 (dubi)

            switch (headerName.ToLowerInvariant()) {

                case "accept":
                    httpWebRequest.Accept = value;
                    break;

                case "connection":
                    SetConnectionHeader(httpWebRequest, value);
                    break;

                case "content-length":
                    httpWebRequest.ContentLength = long.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    break;

                case "content-type":
                    httpWebRequest.ContentType = value;
                    break;

                case "date":
                    httpWebRequest.Date = HttpUtilities.ParseDate(value).DateTime;
                    break;

                case "expect":
                    httpWebRequest.Expect = value;
                    break;

                case "host":
                    httpWebRequest.Host = value;
                    break;

                case "if-modified-since":
                    httpWebRequest.IfModifiedSince = HttpUtilities.ParseDate(value).DateTime;
                    break;

                case "range":
                    SetRangeHeader(httpWebRequest, value);
                    break;

                case "referer":
                    httpWebRequest.Referer = value;
                    break;

                case "transfer-encoding":
                    httpWebRequest.TransferEncoding = value;
                    break;

                case "user-agent":
                    httpWebRequest.UserAgent = value;
                    break;

                default:
                    httpWebRequest.Headers.Set(headerName, value);
                    break;

            }

        }
        public static void SetHeader(this HttpWebRequest httpWebRequest, string headerName, string value) {

            new HttpWebRequestAdapter(httpWebRequest).SetHeader(headerName, value);

        }
        public static void SetHeader(this IHttpWebRequest httpWebRequest, HttpRequestHeader requestHeader, string value) {

            // A list of restricted headers can be found here: https://stackoverflow.com/a/4752359/5383169 (dubi)

            switch (requestHeader) {

                case HttpRequestHeader.Accept:
                    httpWebRequest.SetHeader("accept", value);
                    break;

                case HttpRequestHeader.Connection:
                    httpWebRequest.SetHeader("connection", value);
                    break;

                case HttpRequestHeader.ContentLength:
                    httpWebRequest.SetHeader("content-length", value);
                    break;

                case HttpRequestHeader.ContentType:
                    httpWebRequest.SetHeader("content-type", value);
                    break;

                case HttpRequestHeader.Date:
                    httpWebRequest.SetHeader("date", value);
                    break;

                case HttpRequestHeader.Expect:
                    httpWebRequest.SetHeader("expect", value);
                    break;

                case HttpRequestHeader.Host:
                    httpWebRequest.SetHeader("host", value);
                    break;

                case HttpRequestHeader.IfModifiedSince:
                    httpWebRequest.SetHeader("if-modified-since", value);
                    break;

                case HttpRequestHeader.Range:
                    httpWebRequest.SetHeader("range", value);
                    break;

                case HttpRequestHeader.Referer:
                    httpWebRequest.SetHeader("referer", value);
                    break;

                case HttpRequestHeader.TransferEncoding:
                    httpWebRequest.SetHeader("transfer-encoding", value);
                    break;

                case HttpRequestHeader.UserAgent:
                    httpWebRequest.SetHeader("user-agent", value);
                    break;

                default:
                    httpWebRequest.Headers.Set(requestHeader, value);
                    break;

            }

        }
        public static void SetHeader(this HttpWebRequest httpWebRequest, HttpRequestHeader requestHeader, string value) {

            new HttpWebRequestAdapter(httpWebRequest).SetHeader(requestHeader, value);

        }
        public static void SetHeader(this IHttpWebRequest httpWebRequest, IHttpHeader header) {

            httpWebRequest.SetHeader(header.Name, header.Value);

        }
        public static void SetHeader(this HttpWebRequest httpWebRequest, IHttpHeader header) {

            new HttpWebRequestAdapter(httpWebRequest).SetHeader(header);

        }
        public static void SetHeaders(this IHttpWebRequest httpWebRequest, IEnumerable<IHttpHeader> headers) {

            foreach (IHttpHeader header in headers)
                httpWebRequest.SetHeader(header);

        }
        public static void SetHeaders(this HttpWebRequest httpWebRequest, IEnumerable<IHttpHeader> headers) {

            new HttpWebRequestAdapter(httpWebRequest).SetHeaders(headers);

        }

        public static IHttpWebRequest WithOptions(this IHttpWebRequest httpWebRequest, IHttpWebRequestOptions options, bool copyNullValues = true) {

            if (copyNullValues || !string.IsNullOrWhiteSpace(options.Accept))
                httpWebRequest.Accept = options.Accept;

            if (copyNullValues || !string.IsNullOrWhiteSpace(options.AcceptLanguage))
                httpWebRequest.Headers[HttpRequestHeader.AcceptLanguage] = options.AcceptLanguage;

            if (copyNullValues || options.AllowAutoRedirect.HasValue)
                httpWebRequest.AllowAutoRedirect = options.AllowAutoRedirect ?? httpWebRequest.AllowAutoRedirect;

            httpWebRequest.AutomaticDecompression = options.AutomaticDecompression;

            if (copyNullValues || options.Cookies is object)
                httpWebRequest.CookieContainer = options.Cookies;

            if (copyNullValues || options.Credentials is object)
                httpWebRequest.Credentials = options.Credentials;

            if (httpWebRequest.Headers is object && options.Headers is object)
                httpWebRequest.WithHeaders(options.Headers);

            if (copyNullValues || !string.IsNullOrWhiteSpace(options.Method))
                httpWebRequest.Method = options.Method;

            if (copyNullValues || options.Proxy is object)
                httpWebRequest.Proxy = options.Proxy;

            if (copyNullValues || !string.IsNullOrWhiteSpace(options.UserAgent))
                httpWebRequest.UserAgent = options.UserAgent;

            return httpWebRequest;

        }
        public static HttpWebRequest WithOptions(this HttpWebRequest httpWebRequest, IHttpWebRequestOptions options, bool copyNullValues = true) {

            new HttpWebRequestAdapter(httpWebRequest).WithOptions(options, copyNullValues);

            return httpWebRequest;

        }
        public static IHttpWebRequest WithHeaders(this IHttpWebRequest httpWebRequest, WebHeaderCollection headers) {

            foreach (IHttpHeader header in headers.GetHeaders())
                httpWebRequest.SetHeader(header.Name, header.Value);

            return httpWebRequest;

        }
        public static HttpWebRequest WithHeaders(this HttpWebRequest httpWebRequest, WebHeaderCollection headers) {

            foreach (IHttpHeader header in headers.GetHeaders())
                httpWebRequest.SetHeader(header.Name, header.Value);

            return httpWebRequest;

        }

        // Private members

        private static void SetConnectionHeader(IHttpWebRequest httpWebRequest, string value) {

            // The values "keep-alive" and "close" must not be set directly, or else an exception is thrown.

            if (string.IsNullOrWhiteSpace(value))
                value = "";

            switch (value.Trim().ToLowerInvariant()) {

                case "keep-alive":
                    httpWebRequest.KeepAlive = true;
                    break;

                case "close":
                    httpWebRequest.KeepAlive = false;
                    break;

                default:
                    httpWebRequest.Connection = value;
                    break;

            }

        }
        private static Tuple<long, long> ParseRangeHeader(string headerValue) {

            Match match = Regex.Match(headerValue, @"(\d+)-(\d+)");

            if (!match.Success)
                throw new ArgumentNullException(nameof(headerValue));

            long first = long.Parse(match.Groups[1].Value);
            long second = long.Parse(match.Groups[2].Value);

            return new Tuple<long, long>(first, second);

        }
        private static void SetRangeHeader(IHttpWebRequest httpWebRequest, string value) {

            var range = ParseRangeHeader(value);

            httpWebRequest.AddRange(range.Item1, range.Item2);

        }

    }

}