using System.Net;

namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestOptionsExtensions {

        // Public members

        public static void CopyTo(this IHttpWebRequestOptions options, IHttpWebRequest httpWebRequest, bool copyIfNull = true) {

            if (copyIfNull || !string.IsNullOrEmpty(options.Accept))
                httpWebRequest.Accept = options.Accept;

            if (copyIfNull || !string.IsNullOrEmpty(options.AcceptLanguage))
                httpWebRequest.Headers[HttpRequestHeader.AcceptLanguage] = options.AcceptLanguage;

            httpWebRequest.AutomaticDecompression = options.AutomaticDecompression;

            if (copyIfNull || !(options.Cookies is null))
                httpWebRequest.CookieContainer = options.Cookies;

            if (copyIfNull || !(options.Credentials is null))
                httpWebRequest.Credentials = options.Credentials;

            if (copyIfNull || !(options.Proxy is null))
                httpWebRequest.Proxy = options.Proxy;

            if (copyIfNull || !string.IsNullOrEmpty(options.UserAgent))
                httpWebRequest.UserAgent = options.UserAgent;

        }
        public static void CopyTo(this IHttpWebRequestOptions options, HttpWebRequestOptions other, bool copyIfNull = true) {

            if (copyIfNull || !string.IsNullOrEmpty(options.Accept))
                other.Accept = options.Accept;

            if (copyIfNull || !string.IsNullOrEmpty(options.AcceptLanguage))
                other.AcceptLanguage = options.AcceptLanguage;

            other.AutomaticDecompression = options.AutomaticDecompression;

            if (copyIfNull || !(options.Cookies is null))
                other.Cookies = options.Cookies;

            if (copyIfNull || !(options.Credentials is null))
                other.Credentials = options.Credentials;

            if (copyIfNull || !(options.Proxy is null))
                other.Proxy = options.Proxy;

            if (copyIfNull || !string.IsNullOrEmpty(options.UserAgent))
                other.UserAgent = options.UserAgent;

        }
        public static void CopyTo(this IHttpWebRequestOptions options, WebClient webClient) {

            webClient.Headers[HttpRequestHeader.Accept] = options.Accept;
            webClient.Headers[HttpRequestHeader.AcceptLanguage] = options.AcceptLanguage;
            //webClient.AutomaticDecompression = options.AutomaticDecompression;
            //webClient.CookieContainer = options.Cookies;
            webClient.Credentials = options.Credentials;
            webClient.Proxy = options.Proxy;
            webClient.Headers[HttpRequestHeader.UserAgent] = options.UserAgent;

        }

    }

}