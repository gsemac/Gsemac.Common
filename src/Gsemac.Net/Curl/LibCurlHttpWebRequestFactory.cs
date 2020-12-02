using Gsemac.Net.Extensions;
using System;

namespace Gsemac.Net.Curl {

    public class LibCurlHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public IHttpWebRequestOptions Options { get; set; }

        public LibCurlHttpWebRequestFactory() :
            this(new HttpWebRequestOptions()) {
        }
        public LibCurlHttpWebRequestFactory(IHttpWebRequestOptions options) {

            this.Options = options;

        }

        public IHttpWebRequest CreateHttpWebRequest(Uri requestUri) {

            IHttpWebRequest httpWebRequest = new LibCurlHttpWebRequest(requestUri);

            Options.CopyTo(httpWebRequest);

            return httpWebRequest;

        }

    }

}
