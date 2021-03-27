namespace Gsemac.Net.Curl {

    public class CurlWebClient :
        WebClientBase {

        // Public members

        public CurlWebClient() :
            this(CurlWebRequestOptions.Default) {
        }
        public CurlWebClient(ICurlWebRequestOptions curlOptions) :
           base(new CurlHttpWebRequestFactory(curlOptions)) {
        }
        public CurlWebClient(CurlHttpWebRequestFactory webRequestFactory) :
            base(webRequestFactory) {
        }

    }

}