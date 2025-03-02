using Gsemac.Net.Http;
using System;

namespace Gsemac.Net.Curl {

    [ResponseStreamAlreadyDecompressed]
    internal sealed class CurlProcessHttpWebResponse :
        CurlHttpWebResponseBase {

        // Public members

        internal CurlProcessHttpWebResponse(IHttpWebRequest originatingRequest, CurlProcessStream responseStream) :
            base(originatingRequest, responseStream) {

            if (originatingRequest is null)
                throw new ArgumentNullException(nameof(originatingRequest));

            if (responseStream is null)
                throw new ArgumentNullException(nameof(responseStream));

            ReadHttpHeadersFromStream();

        }

    }

}