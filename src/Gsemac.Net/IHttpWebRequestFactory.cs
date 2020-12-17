using System;

namespace Gsemac.Net {

    public interface IHttpWebRequestFactory {

        IHttpWebRequestOptions GetOptions();
        IHttpWebRequestOptions GetOptions(Uri uri);
        void SetOptions(IHttpWebRequestOptions options);
        void SetOptions(string domain, IHttpWebRequestOptions options);

        IHttpWebRequest CreateHttpWebRequest(Uri requestUri);

    }

}