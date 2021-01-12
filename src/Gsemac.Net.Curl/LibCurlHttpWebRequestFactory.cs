using Gsemac.Net.Extensions;
using System;

namespace Gsemac.Net.Curl {

    public class LibCurlHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public LibCurlHttpWebRequestFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public LibCurlHttpWebRequestFactory(IHttpWebRequestOptions options) :
            this(new HttpWebRequestOptionsFactory(options)) {
        }
        public LibCurlHttpWebRequestFactory(IHttpWebRequestOptionsFactory optionsFactory) {

            this.optionsFactory = optionsFactory;

        }

        public IHttpWebRequest Create(Uri requestUri) {

            return new LibCurlHttpWebRequest(requestUri)
                .WithOptions(optionsFactory.Create(requestUri));

        }

        // Private members

        private readonly IHttpWebRequestOptionsFactory optionsFactory;

    }

}