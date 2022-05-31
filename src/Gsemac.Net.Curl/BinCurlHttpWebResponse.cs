using Gsemac.Net.Http;

namespace Gsemac.Net.Curl {

    internal class BinCurlHttpWebResponse :
        CurlHttpWebResponseBase {

        // Public members

        /// <summary>
        /// Initializes a new instance of the <see cref="BinCurlHttpWebResponse"/> class.
        /// </summary>
        internal BinCurlHttpWebResponse(IHttpWebRequest parentRequest, BinCurlProcessStream responseStream) :
            base(parentRequest, responseStream) {

            ReadHeadersFromResponseStream();

        }

    }

}