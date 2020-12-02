using System.Net;

namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestOptionsExtensions {

        // Public members

        public static void CopyTo(this IHttpWebRequestOptions options, IHttpWebRequest httpWebRequest) {

            httpWebRequest.Accept = options.Accept;
            httpWebRequest.Headers[HttpRequestHeader.AcceptLanguage] = options.AcceptLanguage;
            httpWebRequest.AutomaticDecompression = options.AutomaticDecompression;
            httpWebRequest.CookieContainer = options.Cookies;
            httpWebRequest.Credentials = options.Credentials;
            httpWebRequest.Proxy = options.Proxy;
            httpWebRequest.UserAgent = options.UserAgent;

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