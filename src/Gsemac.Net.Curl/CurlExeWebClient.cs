namespace Gsemac.Net.Curl {

    public class CurlExeWebClient :
        WebClientBase {

        // Public members

        public CurlExeWebClient() :
            this(CurlWebRequestOptions.Default) {
        }
        public CurlExeWebClient(ICurlWebRequestOptions curlOptions) :
            base(new CurlExeHttpWebRequestFactory(curlOptions)) {
        }
        public CurlExeWebClient(CurlExeHttpWebRequestFactory webRequestFactory) :
            base(webRequestFactory) {
        }

    }

}