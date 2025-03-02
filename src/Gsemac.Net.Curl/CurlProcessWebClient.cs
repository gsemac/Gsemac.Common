namespace Gsemac.Net.Curl {

    public class CurlProcessWebClient :
        WebClientBase {

        // Public members

        public CurlProcessWebClient() :
            this(CurlWebRequestOptions.Default) {
        }
        public CurlProcessWebClient(ICurlWebRequestOptions curlOptions) :
            base(new CurlProcessHttpWebRequestFactory(curlOptions)) {
        }
        public CurlProcessWebClient(CurlProcessHttpWebRequestFactory webRequestFactory) :
            base(webRequestFactory) {
        }

    }

}