using System;

namespace Gsemac.Net.Http {

    public interface IHttpWebRequestFactory {

        IHttpWebRequest Create(Uri requestUri);

    }

}