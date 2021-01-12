using System;
using System.IO;

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

            if (File.Exists(LibCurl.LibCurlPath) || !File.Exists(LibCurl.CurlExecutablePath))
                webRequestFactory = new LibCurlHttpWebRequestFactory(optionsFactory);
            else
                webRequestFactory = new BinCurlHttpWebRequestFactory(optionsFactory);

        }

        public IHttpWebRequest Create(Uri requestUri) {

            IHttpWebRequest httpWebRequest = webRequestFactory.Create(requestUri);

            return httpWebRequest;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

    }

}