using System.Net;

namespace Gsemac.Net {

    public class HttpWebRequestOptions :
        IHttpWebRequestOptions {

        public string Accept { get; set; } = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        public string AcceptLanguage { get; set; } = "en-US,en;q=0.5";
        public CookieContainer Cookies { get; set; } = new CookieContainer();
        public IWebProxy Proxy { get; set; }
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36";

    }

}