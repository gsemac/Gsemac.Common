using System;
using System.Net;

namespace Gsemac.Net.Curl {

    /// <summary>
    /// Provides common methods for sending data to and receiving data via Curl from a resource identified by a URI.
    /// </summary>
    public class LibCurlWebClient :
        WebClient {

        // Public members

        /// <summary>
        /// Initializes a new instance of the <see cref="LibCurlWebClient"/> class.
        /// </summary>
        public LibCurlWebClient() { }

        // Protected members

        protected override WebRequest GetWebRequest(Uri address) {

            return new LibCurlHttpWebRequest(address) {
                Credentials = Credentials,
                Headers = Headers,
                Proxy = Proxy
            };

        }

    }

}