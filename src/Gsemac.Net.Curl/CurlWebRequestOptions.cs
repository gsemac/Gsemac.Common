namespace Gsemac.Net.Curl {

    public class CurlWebRequestOptions :
        ICurlWebRequestOptions {

        public static CurlWebRequestOptions Default => new CurlWebRequestOptions();

        public string CurlExecutablePath { get; set; } = CurlUtilities.CurlExecutablePath;

    }

}