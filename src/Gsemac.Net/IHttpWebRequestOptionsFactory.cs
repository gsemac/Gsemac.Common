using System;

namespace Gsemac.Net {

    public interface IHttpWebRequestOptionsFactory {

        IHttpWebRequestOptions Create(Uri requestUri);

    }

}