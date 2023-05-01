using Gsemac.Net.Http;

namespace Gsemac.Net.Curl {

    internal class CurlExeHttpWebResponse :
        CurlHttpWebResponseBase {

        // Public members

        /// <summary>
        /// Initializes a new instance of the <see cref="CurlExeHttpWebResponse"/> class.
        /// </summary>
        internal CurlExeHttpWebResponse(IHttpWebRequest parentRequest, CurlExeProcessStream responseStream) :
            base(parentRequest, responseStream) {

            ReadHeadersFromResponseStream();

        }

    }

}