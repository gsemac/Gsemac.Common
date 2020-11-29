namespace Gsemac.Net.Curl {

    public class BinCurlHttpWebResponse :
        CurlHttpWebResponseBase {

        // Public members

        /// <summary>
        /// Initializes a new instance of the <see cref="BinCurlHttpWebResponse"/> class.
        /// </summary>
        public BinCurlHttpWebResponse(IHttpWebRequest parentRequest, BinCurlProcessStream responseStream) :
            base(parentRequest, responseStream) {
        }

    }

}