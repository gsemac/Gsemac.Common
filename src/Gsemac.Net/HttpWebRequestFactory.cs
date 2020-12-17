using System;

namespace Gsemac.Net {

    public class HttpWebRequestFactory :
        HttpWebRequestFactoryBase {

        // Public members

        public HttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public HttpWebRequestFactory(IHttpWebRequestOptions options) :
            base(options) {
        }

        // Protected members

        protected override IHttpWebRequest CreateHttpWebRequestInternal(Uri requestUri) {

            return new HttpWebRequestWrapper(requestUri);

        }

    }

}