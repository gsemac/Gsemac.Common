using Gsemac.Net.Extensions;
using System;

namespace Gsemac.Net.Curl {

    public class CurlHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public CurlHttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public CurlHttpWebRequestFactory(IHttpWebRequestOptions options) :
            this(new HttpWebRequestOptionsFactory(options)) {
        }
        public CurlHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory) {

            this.optionsFactory = optionsFactory;

        }

        public IHttpWebRequest Create(Uri requestUri) {

            return new CurlHttpWebRequest(requestUri)
                .WithOptions(optionsFactory.Create(requestUri));

        }

        // Private members

        private readonly IHttpWebRequestOptionsFactory optionsFactory;

    }

}