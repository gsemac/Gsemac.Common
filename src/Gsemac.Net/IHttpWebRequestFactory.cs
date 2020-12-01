using System;

namespace Gsemac.Net {

    public interface IHttpWebRequestFactory {

        IHttpWebRequestOptions Options { get; set; }

        IHttpWebRequest CreateHttpWebRequest(Uri requestUri);

    }

}