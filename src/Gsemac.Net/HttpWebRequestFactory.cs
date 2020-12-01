using Gsemac.Net.Extensions;
using System;

namespace Gsemac.Net {

    public class HttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public IHttpWebRequestOptions Options { get; set; }

        public HttpWebRequestFactory() :
            this(new HttpWebRequestOptions()) {
        }
        public HttpWebRequestFactory(IHttpWebRequestOptions options) {

            this.Options = options;

        }

        public IHttpWebRequest CreateHttpWebRequest(Uri uri) {

            IHttpWebRequest httpWebRequest = new HttpWebRequestWrapper(uri);

            Options.CopyTo(httpWebRequest);

            return httpWebRequest;

        }

    }

}