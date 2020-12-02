using System;
using System.IO;

namespace Gsemac.Net.Curl {

    public class CurlHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public IHttpWebRequestOptions Options {
            get => webRequestFactory.Options;
            set => webRequestFactory.Options = value;
        }

        public CurlHttpWebRequestFactory() :
            this(new HttpWebRequestOptions()) {
        }
        public CurlHttpWebRequestFactory(IHttpWebRequestOptions options) {

            if (File.Exists(LibCurl.LibCurlPath) || !File.Exists(LibCurl.CurlExecutablePath))
                webRequestFactory = new LibCurlHttpWebRequestFactory(options);
            else
                webRequestFactory = new BinCurlHttpWebRequestFactory(options);

        }

        public IHttpWebRequest CreateHttpWebRequest(Uri requestUri) {

            IHttpWebRequest httpWebRequest = webRequestFactory.CreateHttpWebRequest(requestUri);

            return httpWebRequest;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

    }

}