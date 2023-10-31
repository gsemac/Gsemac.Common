using Gsemac.Net.Http;
using System;

namespace Gsemac.Net.Curl {

    internal class CurlExeHttpWebResponse :
        CurlHttpWebResponseBase {

        // Public members

        internal CurlExeHttpWebResponse(IHttpWebRequest originatingRequest, CurlExeProcessStream responseStream) :
            base(originatingRequest, responseStream) {

            if (originatingRequest is null)
                throw new ArgumentNullException(nameof(originatingRequest));

            if (responseStream is null)
                throw new ArgumentNullException(nameof(responseStream));

            ReadHttpHeadersFromStream();

        }

    }

}