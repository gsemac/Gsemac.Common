using System.Net;

namespace Gsemac.Net.Http {

    public interface IHttpWebRequestOptions {

        string Accept { get; }
        string AcceptLanguage { get; }
        bool? AllowAutoRedirect { get; }
        DecompressionMethods AutomaticDecompression { get; }
        CookieContainer Cookies { get; }
        ICredentials Credentials { get; }
        WebHeaderCollection Headers { get; }
        string Method { get; }
        IWebProxy Proxy { get; }
        string UserAgent { get; }

    }

}