using Gsemac.Net.Extensions;
using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using System;

namespace Gsemac.Net.Curl {

    public class CurlExeHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public CurlExeHttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public CurlExeHttpWebRequestFactory(ICurlWebRequestOptions curlOptions) :
            this() {

            this.curlOptions = curlOptions;

        }
        public CurlExeHttpWebRequestFactory(IHttpWebRequestOptions options) :
            this(new HttpWebRequestOptionsFactory(options)) {
        }
        public CurlExeHttpWebRequestFactory(IHttpWebRequestOptions options, ICurlWebRequestOptions curlOptions) :
            this(new HttpWebRequestOptionsFactory(options), curlOptions) {
        }
        public CurlExeHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory) {

            this.optionsFactory = optionsFactory;

        }
        public CurlExeHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory, ICurlWebRequestOptions curlOptions) :
            this(optionsFactory) {

            this.curlOptions = curlOptions;

        }

        public IHttpWebRequest Create(Uri requestUri) {

            return (string.IsNullOrWhiteSpace(curlOptions.CurlExecutablePath) ?
                new CurlExeHttpWebRequest(requestUri) :
                new CurlExeHttpWebRequest(requestUri, curlOptions))
                .WithOptions(optionsFactory.Create(requestUri));

        }

        // Private members

        private readonly ICurlWebRequestOptions curlOptions = CurlWebRequestOptions.Default;
        private readonly IHttpWebRequestOptionsFactory optionsFactory;

    }

}