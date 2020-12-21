using System;

namespace Gsemac.Net.Curl {

    public class BinCurlHttpWebRequestFactory :
        HttpWebRequestFactoryBase {

        // Public members

        public BinCurlHttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public BinCurlHttpWebRequestFactory(string curlExecutablePath) :
            this() {

            this.curlExecutablePath = curlExecutablePath;

        }
        public BinCurlHttpWebRequestFactory(IHttpWebRequestOptions options) :
            base(options) {
        }
        public BinCurlHttpWebRequestFactory(IHttpWebRequestOptions options, string curlExecutablePath) :
            this(options) {

            this.curlExecutablePath = curlExecutablePath;

        }

        // Protected members

        protected override IHttpWebRequest CreateInternal(Uri requestUri) {

            return string.IsNullOrWhiteSpace(curlExecutablePath) ?
                new BinCurlHttpWebRequest(requestUri) :
                new BinCurlHttpWebRequest(requestUri, curlExecutablePath);

        }

        // Private members

        private readonly string curlExecutablePath;

    }

}