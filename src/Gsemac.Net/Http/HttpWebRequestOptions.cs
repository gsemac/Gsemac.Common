using Gsemac.Net.Extensions;
using Gsemac.Net.Properties;
using System;
using System.Net;

namespace Gsemac.Net.Http {

    public class HttpWebRequestOptions :
        IHttpWebRequestOptions {

        // Public members

        public string Accept {
            get => GetHeader(HttpRequestHeader.Accept);
            set => SetHeader(HttpRequestHeader.Accept, value, removeIfEmpty: true);
        }
        public string AcceptLanguage {
            get => GetHeader(HttpRequestHeader.AcceptLanguage);
            set => SetHeader(HttpRequestHeader.AcceptLanguage, value, removeIfEmpty: true);
        }
        public bool? AllowAutoRedirect { get; set; } = true;
        public DecompressionMethods AutomaticDecompression { get; set; } = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        public CookieContainer Cookies { get; set; }
        public ICredentials Credentials { get; set; }
        public WebHeaderCollection Headers {
            get => headers;
            set => headers = value.Clone();
        }
        public string Method { get; set; } = DefaultHttpHeaders.Method;
        public IWebProxy Proxy { get; set; } = WebProxyUtilities.GetDefaultProxy();
        public string UserAgent {
            get => GetHeader(HttpRequestHeader.UserAgent);
            set => SetHeader(HttpRequestHeader.UserAgent, value, removeIfEmpty: true);
        }

        public static HttpWebRequestOptions Default => new HttpWebRequestOptions();
        public static HttpWebRequestOptions Empty {
            get {

                return new HttpWebRequestOptions() {
                    Accept = string.Empty,
                    AcceptLanguage = string.Empty,
                    AutomaticDecompression = DecompressionMethods.None,
                    Cookies = null,
                    Credentials = null,
                    Method = string.Empty,
                    Proxy = null,
                    UserAgent = string.Empty,
                };

            }
        }

        public HttpWebRequestOptions() {

            Accept = DefaultHttpHeaders.Accept;
            AcceptLanguage = DefaultHttpHeaders.AcceptLanguage;
            UserAgent = DefaultHttpHeaders.UserAgent;

        }
        public HttpWebRequestOptions(IHttpWebRequestOptions other, bool copyNullValues = true) {

            Combine(other, copyNullValues);

        }

        public static HttpWebRequestOptions FromHttpWebRequest(IHttpWebRequest httpWebRequest) {

            if (httpWebRequest is null)
                throw new ArgumentNullException(nameof(httpWebRequest));

            return new HttpWebRequestOptions() {
                Accept = httpWebRequest.Accept,
                AcceptLanguage = httpWebRequest.Headers[HttpRequestHeader.AcceptLanguage],
                AllowAutoRedirect = httpWebRequest.AllowAutoRedirect,
                AutomaticDecompression = httpWebRequest.AutomaticDecompression,
                Cookies = httpWebRequest.CookieContainer,
                Credentials = httpWebRequest.Credentials,
                Headers = httpWebRequest.Headers.Clone(),
                Method = httpWebRequest.Method,
                Proxy = httpWebRequest.Proxy,
                UserAgent = httpWebRequest.UserAgent,
            };

        }
        public static HttpWebRequestOptions FromHttpWebRequest(HttpWebRequest httpWebRequest) {

            return FromHttpWebRequest(new HttpWebRequestAdapter(httpWebRequest));

        }

        public static HttpWebRequestOptions Combine(IHttpWebRequestOptions options1, IHttpWebRequestOptions options2, bool copyNullValues = true) {

            if (options1 is null)
                throw new ArgumentNullException(nameof(options1));

            if (options2 is null)
                throw new ArgumentNullException(nameof(options2));

            HttpWebRequestOptions result = new HttpWebRequestOptions(options1);

            result.Combine(options2, copyNullValues);

            return result;

        }

        // Private members

        private WebHeaderCollection headers = new WebHeaderCollection();

        private void Combine(IHttpWebRequestOptions other, bool copyNullValues = true) {

            if (copyNullValues || !string.IsNullOrWhiteSpace(other.Accept))
                Accept = other.Accept;

            if (copyNullValues || !string.IsNullOrWhiteSpace(other.AcceptLanguage))
                AcceptLanguage = other.AcceptLanguage;

            if (copyNullValues || other.AllowAutoRedirect.HasValue)
                AllowAutoRedirect = other.AllowAutoRedirect;

            if (copyNullValues || other.AutomaticDecompression != DecompressionMethods.None)
                AutomaticDecompression = other.AutomaticDecompression;

            if (copyNullValues || other.Cookies is object)
                Cookies = other.Cookies;

            if (copyNullValues || other.Credentials is object)
                Credentials = other.Credentials;

            if (Headers is null) {

                Headers = other.Headers.Clone();

            }
            else if (other.Headers is object) {

                other.Headers.CopyTo(Headers);

            }

            if (copyNullValues || !string.IsNullOrWhiteSpace(other.Method))
                Method = other.Method;

            if (copyNullValues || other.Proxy is object)
                Proxy = other.Proxy;

            if (copyNullValues || !string.IsNullOrWhiteSpace(other.UserAgent))
                UserAgent = other.UserAgent;

        }

        private string GetHeader(HttpRequestHeader header) {

            if (Headers is object && Headers.TryGet(header, out string value))
                return value;

            return string.Empty;

        }
        private void SetHeader(HttpRequestHeader header, string value, bool removeIfEmpty) {

            if (Headers is null)
                Headers = new WebHeaderCollection();

            if (removeIfEmpty && string.IsNullOrEmpty(value))
                Headers.Remove(header);
            else
                Headers.TrySet(header, value);

        }

    }

}