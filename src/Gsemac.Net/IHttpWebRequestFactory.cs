using System;

namespace Gsemac.Net {

    public interface IHttpWebRequestFactory {

        IHttpWebRequest Create(Uri requestUri);

    }

}