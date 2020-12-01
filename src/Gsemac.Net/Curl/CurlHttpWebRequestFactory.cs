using Gsemac.Net.Extensions;
using System;
using System.IO;

namespace Gsemac.Net.Curl {

    public class CurlHttpWebRequestFactory :
        IHttpWebRequestFactory {

        // Public members

        public IHttpWebRequestOptions Options { get; set; }

        public CurlHttpWebRequestFactory() :
            this(new HttpWebRequestOptions()) {
        }
        public CurlHttpWebRequestFactory(IHttpWebRequestOptions options) {

            this.Options = options;

            useLibCurl = File.Exists(LibCurl.LibCurlPath) || !File.Exists(LibCurl.CurlExecutablePath);

        }

        public IHttpWebRequest CreateHttpWebRequest(Uri requestUri) {

            IHttpWebRequest httpWebRequest;

            if (useLibCurl)
                httpWebRequest = new LibCurlHttpWebRequest(requestUri);
            else
                httpWebRequest = new BinCurlHttpWebRequest(requestUri);

            Options.CopyTo(httpWebRequest);

            return httpWebRequest;

        }

        // Private members

        private readonly bool useLibCurl;

    }

}