using System.Net;

namespace Gsemac.Net {

    public interface IHttpWebRequestOptions {

        string Accept { get; }
        string AcceptLanguage { get; }
        bool? AllowAutoRedirect { get; }
        DecompressionMethods AutomaticDecompression { get; }
        CookieContainer Cookies { get; }
        ICredentials Credentials { get; }
        IWebProxy Proxy { get; }
        string UserAgent { get; }

    }

}