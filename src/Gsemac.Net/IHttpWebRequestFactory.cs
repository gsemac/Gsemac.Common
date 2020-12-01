using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Net {

    public interface IHttpWebRequestFactory {

        IHttpWebRequest CreateHttpWebRequest();

    }

}