using System.Net;

namespace Gsemac.Net.Extensions {

    public static class HttpWebRequestOptionsExtensions {

        // Public members

        public static void CopyTo(this IHttpWebRequestOptions options, IHttpWebRequest httpWebRequest) {

            httpWebRequest.Accept = options.Accept;
            httpWebRequest.Headers[HttpRequestHeader.AcceptLanguage] = options.AcceptLanguage;
            httpWebRequest.AutomaticDecompression = options.AutomaticDecompression;
            httpWebRequest.CookieContainer = options.Cookies;
            httpWebRequest.Proxy = options.Proxy;
            httpWebRequest.UserAgent = options.UserAgent;

        }

    }

}