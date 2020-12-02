using Gsemac.Net.Extensions;
using System;

namespace Gsemac.Net.Curl {

    public class BinCurlHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public IHttpWebRequestOptions Options { get; set; }

        public BinCurlHttpWebRequestFactory() :
            this(new HttpWebRequestOptions()) {
        }
        public BinCurlHttpWebRequestFactory(string curlExecutablePath) :
            this() {

            this.curlExecutablePath = curlExecutablePath;

        }
        public BinCurlHttpWebRequestFactory(IHttpWebRequestOptions options) {

            this.Options = options;

        }
        public BinCurlHttpWebRequestFactory(IHttpWebRequestOptions options, string curlExecutablePath) :
            this(options) {

            this.curlExecutablePath = curlExecutablePath;

        }

        public IHttpWebRequest CreateHttpWebRequest(Uri requestUri) {

            IHttpWebRequest httpWebRequest = string.IsNullOrWhiteSpace(curlExecutablePath) ?
                new BinCurlHttpWebRequest(requestUri) :
                new BinCurlHttpWebRequest(requestUri, curlExecutablePath);

            Options.CopyTo(httpWebRequest);

            return httpWebRequest;

        }

        // Private members

        private readonly string curlExecutablePath;

    }

}