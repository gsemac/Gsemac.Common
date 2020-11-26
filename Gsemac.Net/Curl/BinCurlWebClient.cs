using System;
using System.Net;

namespace Gsemac.Net.Curl {

    /// <summary>
    /// Provides common methods for sending data to and receiving data via Curl from a resource identified by a URI.
    /// </summary>
    public class BinCurlWebClient :
        WebClient {

        // Public members

        public BinCurlWebClient() :
            this(LibCurl.CurlExecutablePath) {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BinCurlWebClient"/> class.
        /// </summary>
        /// <param name="curlExecutablePath">Path to Curl executable.</param>
        public BinCurlWebClient(string curlExecutablePath) {

            this.curlExecutablePath = curlExecutablePath;

        }

        // Protected members

        protected override WebRequest GetWebRequest(Uri address) {

            return new BinCurlHttpWebRequest(address, curlExecutablePath) {
                Credentials = Credentials,
                Headers = Headers,
                Proxy = Proxy
            };

        }

        // Private members

        private readonly string curlExecutablePath;

    }

}