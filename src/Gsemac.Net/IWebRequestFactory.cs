using System;

namespace Gsemac.Net {

    public interface IWebRequestFactory {

        IWebRequest Create(Uri requestUri);

    }

}