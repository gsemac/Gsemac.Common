using System;

namespace Gsemac.Net {

    public interface IHttpWebRequestOptionsFactory {

        IHttpWebRequestOptions Create();
        IHttpWebRequestOptions Create(Uri requestUri);

    }

}