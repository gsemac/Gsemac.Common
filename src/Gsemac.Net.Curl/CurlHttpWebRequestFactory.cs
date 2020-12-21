using System;
using System.IO;

namespace Gsemac.Net.Curl {

    public class CurlHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public IHttpWebRequestOptions GetOptions() {

            return webRequestFactory.GetOptions();

        }
        public IHttpWebRequestOptions GetOptions(Uri uri) {

            return webRequestFactory.GetOptions(uri);

        }
        public void SetOptions(IHttpWebRequestOptions options) {

            webRequestFactory.SetOptions(options);

        }
        public void SetOptions(string domain, IHttpWebRequestOptions options) {

            webRequestFactory.SetOptions(domain, options);

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

        public IHttpWebRequest Create(Uri requestUri) {

            IHttpWebRequest httpWebRequest = webRequestFactory.Create(requestUri);

            return httpWebRequest;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

    }

}