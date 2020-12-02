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
        public BinCurlHttpWebRequestFactory(IHttpWebRequestOptions options) {

            this.Options = options;

        }

        public IHttpWebRequest CreateHttpWebRequest(Uri requestUri) {

            IHttpWebRequest httpWebRequest = new BinCurlHttpWebRequest(requestUri);

            Options.CopyTo(httpWebRequest);

            return httpWebRequest;

        }

    }

}
