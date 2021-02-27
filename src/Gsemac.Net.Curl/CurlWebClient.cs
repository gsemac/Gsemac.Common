namespace Gsemac.Net.Curl {

    /// <summary>
    /// Provides common methods for sending data to and receiving data via Curl from a resource identified by a URI.
    /// </summary>
    public class CurlWebClient :
        WebClientBase {

        // Public members

        /// <summary>
        /// Initializes a new instance of the <see cref="CurlWebClient"/> class.
        /// </summary>
        public CurlWebClient() :
            base(new CurlHttpWebRequestFactory()) {
        }

    }

}