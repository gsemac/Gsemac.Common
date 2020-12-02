namespace Gsemac.Net.Curl {

    /// <summary>
    /// Provides common methods for sending data to and receiving data via Curl from a resource identified by a URI.
    /// </summary>
    public class LibCurlWebClient :
        WebClientBase {

        // Public members

        /// <summary>
        /// Initializes a new instance of the <see cref="LibCurlWebClient"/> class.
        /// </summary>
        public LibCurlWebClient() :
            base(new LibCurlHttpWebRequestFactory()) {
        }

    }

}