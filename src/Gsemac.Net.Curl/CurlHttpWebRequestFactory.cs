using Gsemac.Net.Extensions;
using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using System;

namespace Gsemac.Net.Curl {

    public sealed class CurlHttpWebRequestFactory :
        ICurlHttpWebRequestFactory {

        // Public members

        public CurlHttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public CurlHttpWebRequestFactory(IHttpWebRequestOptions options) :
            this(new HttpWebRequestOptionsFactory(options)) {
        }
        public CurlHttpWebRequestFactory(ICurlWebRequestOptions curlOptions) :
            this() {

            this.curlOptions = curlOptions;

        }
        public CurlHttpWebRequestFactory(IHttpWebRequestOptions options, ICurlWebRequestOptions curlOptions) :
           this(new HttpWebRequestOptionsFactory(options), curlOptions) {
        }
        public CurlHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory) {

            this.optionsFactory = optionsFactory;

        }
        public CurlHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory, ICurlWebRequestOptions curlOptions) :
            this(optionsFactory) {

            this.curlOptions = curlOptions;

        }

        public IHttpWebRequest Create(Uri requestUri) {

            return new CurlHttpWebRequest(requestUri, curlOptions)
                .WithOptions(optionsFactory.Create(requestUri));

        }

        // Private members

        private readonly ICurlWebRequestOptions curlOptions = CurlWebRequestOptions.Default;
        private readonly IHttpWebRequestOptionsFactory optionsFactory;

    }

}