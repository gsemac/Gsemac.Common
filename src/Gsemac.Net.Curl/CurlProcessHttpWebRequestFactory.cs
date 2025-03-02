using Gsemac.Net.Extensions;
using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using System;

namespace Gsemac.Net.Curl {

    public class CurlProcessHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public CurlProcessHttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public CurlProcessHttpWebRequestFactory(ICurlWebRequestOptions curlOptions) :
            this() {

            this.curlOptions = curlOptions;

        }
        public CurlProcessHttpWebRequestFactory(IHttpWebRequestOptions options) :
            this(new HttpWebRequestOptionsFactory(options)) {
        }
        public CurlProcessHttpWebRequestFactory(IHttpWebRequestOptions options, ICurlWebRequestOptions curlOptions) :
            this(new HttpWebRequestOptionsFactory(options), curlOptions) {
        }
        public CurlProcessHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory) {

            this.optionsFactory = optionsFactory;

        }
        public CurlProcessHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory, ICurlWebRequestOptions curlOptions) :
            this(optionsFactory) {

            this.curlOptions = curlOptions;

        }

        public IHttpWebRequest Create(Uri requestUri) {

            return (string.IsNullOrWhiteSpace(curlOptions.CurlExecutablePath) ?
                new CurlProcessHttpWebRequest(requestUri) :
                new CurlProcessHttpWebRequest(requestUri, curlOptions))
                .WithOptions(optionsFactory.Create(requestUri));

        }

        // Private members

        private readonly ICurlWebRequestOptions curlOptions = CurlWebRequestOptions.Default;
        private readonly IHttpWebRequestOptionsFactory optionsFactory;

    }

}