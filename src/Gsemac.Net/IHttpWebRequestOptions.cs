using System.Net;

namespace Gsemac.Net {

    public interface IHttpWebRequestOptions {

        string Accept { get; }
        string AcceptLanguage { get; }
        CookieContainer Cookies { get; }
        IWebProxy Proxy { get; }
        string UserAgent { get; }

    }

}