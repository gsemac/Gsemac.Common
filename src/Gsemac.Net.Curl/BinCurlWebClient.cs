namespace Gsemac.Net.Curl {

    public class BinCurlWebClient :
        WebClientBase {

        // Public members

        public BinCurlWebClient() :
            this(CurlWebRequestOptions.Default) {
        }
        public BinCurlWebClient(ICurlWebRequestOptions curlOptions) :
            base(new BinCurlHttpWebRequestFactory(curlOptions)) {
        }
        public BinCurlWebClient(BinCurlHttpWebRequestFactory webRequestFactory) :
            base(webRequestFactory) {
        }

    }

}