using Gsemac.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Extensions {

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
                    httpWebRequest.Date = DateUtilities.ParseHttpHeader(value).DateTime;
                    break;

                case "expect":
                    httpWebRequest.Expect = value;
                    break;

                case "host":
                    httpWebRequest.Host = value;
                    break;

                case "if-modified-since":
                    httpWebRequest.IfModifiedSince = DateUtilities.ParseHttpHeader(value).DateTime;
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

            SetHeader(new HttpWebRequestAdapter(httpWebRequest), headerName, value);

        }
        public static void SetHeader(this IHttpWebRequest httpWebRequest, HttpRequestHeader requestHeader, string value) {

            // A list of restricted headers can be found here: https://stackoverflow.com/a/4752359/5383169 (dubi)

            switch (requestHeader) {

                case HttpRequestHeader.Accept:
                    SetHeader(httpWebRequest, "accept", value);
                    break;

                case HttpRequestHeader.Connection:
                    SetHeader(httpWebRequest, "connection", value);
                    break;

                case HttpRequestHeader.ContentLength:
                    SetHeader(httpWebRequest, "content-length", value);
                    break;

                case HttpRequestHeader.ContentType:
                    SetHeader(httpWebRequest, "content-type", value);
                    break;

                case HttpRequestHeader.Date:
                    SetHeader(httpWebRequest, "date", value);
                    break;

                case HttpRequestHeader.Expect:
                    SetHeader(httpWebRequest, "expect", value);
                    break;

                case HttpRequestHeader.Host:
                    SetHeader(httpWebRequest, "host", value);
                    break;

                case HttpRequestHeader.IfModifiedSince:
                    SetHeader(httpWebRequest, "if-modified-since", value);
                    break;

                case HttpRequestHeader.Range:
                    SetHeader(httpWebRequest, "range", value);
                    break;

                case HttpRequestHeader.Referer:
                    SetHeader(httpWebRequest, "referer", value);
                    break;

                case HttpRequestHeader.TransferEncoding:
                    SetHeader(httpWebRequest, "transfer-encoding", value);
                    break;

                case HttpRequestHeader.UserAgent:
                    SetHeader(httpWebRequest, "user-agent", value);
                    break;

                default:
                    httpWebRequest.Headers.Set(requestHeader, value);
                    break;

            }

        }
        public static void SetHeader(this HttpWebRequest httpWebRequest, HttpRequestHeader requestHeader, string value) {

            SetHeader(new HttpWebRequestAdapter(httpWebRequest), requestHeader, value);

        }
        public static void SetHeader(this IHttpWebRequest httpWebRequest, IHttpHeader header) {

            SetHeader(httpWebRequest, header.Name, header.Value);

        }
        public static void SetHeader(this HttpWebRequest httpWebRequest, IHttpHeader header) {

            SetHeader(new HttpWebRequestAdapter(httpWebRequest), header);

        }
        public static void SetHeaders(this IHttpWebRequest httpWebRequest, IEnumerable<IHttpHeader> headers) {

            foreach (IHttpHeader header in headers)
                SetHeader(httpWebRequest, header);

        }
        public static void SetHeaders(this HttpWebRequest httpWebRequest, IEnumerable<IHttpHeader> headers) {

            SetHeaders(new HttpWebRequestAdapter(httpWebRequest), headers);

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

            if (httpWebRequest.Headers is object && options.Headers is object)
                httpWebRequest.WithHeaders(options.Headers);

            if (copyIfNull || options.Proxy is object)
                httpWebRequest.Proxy = options.Proxy;

            if (copyIfNull || !string.IsNullOrWhiteSpace(options.UserAgent))
                httpWebRequest.UserAgent = options.UserAgent;

            return httpWebRequest;

        }
        public static HttpWebRequest WithOptions(this HttpWebRequest httpWebRequest, IHttpWebRequestOptions options, bool copyIfNull = true) {

            WithOptions(new HttpWebRequestAdapter(httpWebRequest), options, copyIfNull);

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

        public static void CopyTo(this IHttpWebRequest httpWebRequest, IHttpWebRequest other) {

            other.Method = httpWebRequest.Method;
            other.AuthenticationLevel = httpWebRequest.AuthenticationLevel;
            other.Timeout = httpWebRequest.Timeout;
            other.PreAuthenticate = httpWebRequest.PreAuthenticate;
            other.Proxy = httpWebRequest.Proxy;
            other.UseDefaultCredentials = httpWebRequest.UseDefaultCredentials;
            other.Credentials = httpWebRequest.Credentials;
            other.ContentType = httpWebRequest.ContentType;

            if (httpWebRequest.ContentLength >= 0) // HttpWebRequest will throw an exception if copying unitialized ContentLength
                other.ContentLength = httpWebRequest.ContentLength;

            other.Headers = httpWebRequest.Headers;
            other.ConnectionGroupName = httpWebRequest.ConnectionGroupName;
            other.ImpersonationLevel = httpWebRequest.ImpersonationLevel;
            other.CachePolicy = httpWebRequest.CachePolicy;

            other.Accept = httpWebRequest.Accept;
            other.Expect = httpWebRequest.Expect;
            other.ClientCertificates = httpWebRequest.ClientCertificates;
            other.CookieContainer = httpWebRequest.CookieContainer;
            other.ReadWriteTimeout = httpWebRequest.ReadWriteTimeout;
            other.ContinueDelegate = httpWebRequest.ContinueDelegate;
            other.Host = httpWebRequest.Host;
            other.Referer = httpWebRequest.Referer;
            other.MaximumAutomaticRedirections = httpWebRequest.MaximumAutomaticRedirections;
            other.MaximumResponseHeadersLength = httpWebRequest.MaximumResponseHeadersLength;
            other.ProtocolVersion = httpWebRequest.ProtocolVersion;
            other.UserAgent = httpWebRequest.UserAgent;
            other.MediaType = httpWebRequest.MediaType;
            other.Connection = httpWebRequest.Connection;
            other.Date = httpWebRequest.Date;
            other.AutomaticDecompression = httpWebRequest.AutomaticDecompression;
            other.SendChunked = httpWebRequest.SendChunked;
            other.UnsafeAuthenticatedConnectionSharing = httpWebRequest.UnsafeAuthenticatedConnectionSharing;
            other.Pipelined = httpWebRequest.Pipelined;
            other.KeepAlive = httpWebRequest.KeepAlive;
            other.AllowWriteStreamBuffering = httpWebRequest.AllowWriteStreamBuffering;
            other.AllowAutoRedirect = httpWebRequest.AllowAutoRedirect;
            other.IfModifiedSince = httpWebRequest.IfModifiedSince;

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