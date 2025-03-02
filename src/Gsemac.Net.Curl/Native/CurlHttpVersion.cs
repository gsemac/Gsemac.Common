namespace Gsemac.Net.Curl.Native {

    // https://github.com/curl/curl/blob/0d16a49c16a868524a3e51d390b5ea106ce9b51c/include/curl/curl.h#L2119

    public enum CurlHttpVersion {

        None,
        Http10,
        Http11,
        Http2,
        Http2Tls,
        Http2PriorKnowledge,
        Http3 = 30

    }

}