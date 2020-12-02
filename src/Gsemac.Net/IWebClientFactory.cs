using System.Net;

namespace Gsemac.Net {

    public interface IWebClientFactory {

        IHttpWebRequestOptions Options { get; set; }

        WebClient CreateWebClient();

    }

}