using Gsemac.Net.Extensions;
using System;

namespace Gsemac.Net.Curl {

    public class BinCurlHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public BinCurlHttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public BinCurlHttpWebRequestFactory(ICurlWebRequestOptions curlOptions) :
            this() {

            this.curlOptions = curlOptions;

        }
        public BinCurlHttpWebRequestFactory(IHttpWebRequestOptions options) :
            this(new HttpWebRequestOptionsFactory(options)) {
        }
        public BinCurlHttpWebRequestFactory(IHttpWebRequestOptions options, ICurlWebRequestOptions curlOptions) :
            this(new HttpWebRequestOptionsFactory(options), curlOptions) {
        }
        public BinCurlHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory) {

            this.optionsFactory = optionsFactory;

        }
        public BinCurlHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory, ICurlWebRequestOptions curlOptions) :
            this(optionsFactory) {

            this.curlOptions = curlOptions;

        }

        public IHttpWebRequest Create(Uri requestUri) {

            return (string.IsNullOrWhiteSpace(curlOptions.CurlExecutablePath) ?
                new BinCurlHttpWebRequest(requestUri) :
                new BinCurlHttpWebRequest(requestUri, curlOptions.CurlExecutablePath))
                .WithOptions(optionsFactory.Create(requestUri));

        }

        // Private members

        private readonly ICurlWebRequestOptions curlOptions = CurlWebRequestOptions.Default;
        private readonly IHttpWebRequestOptionsFactory optionsFactory;

    }

}