using System;

namespace Gsemac.Net.Http {

    public interface IHttpWebRequestOptionsFactory {

        IHttpWebRequestOptions Create();
        IHttpWebRequestOptions Create(Uri requestUri);

    }

}