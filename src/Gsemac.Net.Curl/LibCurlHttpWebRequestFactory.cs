using System;

namespace Gsemac.Net.Curl {

    public class LibCurlHttpWebRequestFactory :
        HttpWebRequestFactoryBase {

        // Public members

        public LibCurlHttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public LibCurlHttpWebRequestFactory(IHttpWebRequestOptions options) :
            base(options) {
        }

        // Protected members

        protected override IHttpWebRequest CreateHttpWebRequestInternal(Uri requestUri) {

            return new LibCurlHttpWebRequest(requestUri);

        }

    }

}