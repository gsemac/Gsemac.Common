namespace Gsemac.Net.Curl {

    /// <summary>
    /// Provides common methods for sending data to and receiving data via Curl from a resource identified by a URI.
    /// </summary>
    public class BinCurlWebClient :
        WebClientBase {

        // Public members

        public BinCurlWebClient() :
            this(LibCurl.CurlExecutablePath) {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BinCurlWebClient"/> class.
        /// </summary>
        /// <param name="curlExecutablePath">Path to Curl executable.</param>
        public BinCurlWebClient(string curlExecutablePath) :
            base(new BinCurlHttpWebRequestFactory(curlExecutablePath)) {
        }

    }

}