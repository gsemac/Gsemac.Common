using Gsemac.Net.Extensions;
using System.Net;

namespace Gsemac.Net {

    public class HttpWebRequestOptions :
        IHttpWebRequestOptions {

        // Public members

        public string Accept { get; set; } = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        public string AcceptLanguage { get; set; } = "en-US,en;q=0.5";
        public bool? AllowAutoRedirect { get; set; } = true;
        public DecompressionMethods AutomaticDecompression { get; set; } = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        public CookieContainer Cookies { get; set; } = new CookieContainer();
        public ICredentials Credentials { get; set; }
        public WebHeaderCollection Headers { get; set; } = new WebHeaderCollection();
        public IWebProxy Proxy { get; set; } = WebProxyUtilities.GetDefaultProxy();
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36";

        public static HttpWebRequestOptions Default => new HttpWebRequestOptions();
        public static HttpWebRequestOptions Empty {
            get {

                return new HttpWebRequestOptions() {
                    Accept = string.Empty,
                    AcceptLanguage = string.Empty,
                    AutomaticDecompression = DecompressionMethods.None,
                    Cookies = null,
                    Credentials = null,
                    Proxy = null,
                    UserAgent = string.Empty,
                };

            }
        }

        public HttpWebRequestOptions() {
        }
        public HttpWebRequestOptions(IHttpWebRequestOptions other, bool copyIfNull = true) {

            Combine(other, copyIfNull);

        }

        public static HttpWebRequestOptions FromHttpWebRequest(IHttpWebRequest httpWebRequest) {

            return new HttpWebRequestOptions() {
                Accept = httpWebRequest.Accept,
                AcceptLanguage = httpWebRequest.Headers[HttpRequestHeader.AcceptLanguage],
                AllowAutoRedirect = httpWebRequest.AllowAutoRedirect,
                AutomaticDecompression = httpWebRequest.AutomaticDecompression,
                Cookies = httpWebRequest.CookieContainer,
                Credentials = httpWebRequest.Credentials,
                Headers = httpWebRequest.Headers.Clone(),
                Proxy = httpWebRequest.Proxy,
                UserAgent = httpWebRequest.UserAgent,
            };

        }
        public static HttpWebRequestOptions FromHttpWebRequest(HttpWebRequest httpWebRequest) {

            return FromHttpWebRequest(new HttpWebRequestAdapter(httpWebRequest));

        }

        public static HttpWebRequestOptions Combine(IHttpWebRequestOptions options1, IHttpWebRequestOptions options2, bool copyIfNull = true) {

            HttpWebRequestOptions result = new HttpWebRequestOptions(options1);

            result.Combine(options2, copyIfNull);

            return result;

        }

        // Private members

        private void Combine(IHttpWebRequestOptions other, bool copyIfNull = true) {

            if (copyIfNull || !string.IsNullOrWhiteSpace(other.Accept))
                Accept = other.Accept;

            if (copyIfNull || !string.IsNullOrWhiteSpace(other.AcceptLanguage))
                AcceptLanguage = other.AcceptLanguage;

            if (copyIfNull || other.AllowAutoRedirect.HasValue)
                AllowAutoRedirect = other.AllowAutoRedirect;

            if (copyIfNull || other.AutomaticDecompression != DecompressionMethods.None)
                AutomaticDecompression = other.AutomaticDecompression;

            if (copyIfNull || other.Cookies is object)
                Cookies = other.Cookies;

            if (copyIfNull || other.Credentials is object)
                Credentials = other.Credentials;

            if (Headers is null)
                Headers = other.Headers.Clone();
            else if (other.Headers is object)
                other.Headers.CopyTo(Headers);

            if (copyIfNull || other.Proxy is object)
                Proxy = other.Proxy;

            if (copyIfNull || !string.IsNullOrWhiteSpace(other.UserAgent))
                UserAgent = other.UserAgent;

        }

    }

}