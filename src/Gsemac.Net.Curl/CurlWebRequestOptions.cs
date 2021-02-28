namespace Gsemac.Net.Curl {

    public class CurlWebRequestOptions :
        ICurlWebRequestOptions {

        public static CurlWebRequestOptions Default => new CurlWebRequestOptions();

        public string CABundlePath { get; set; } = CurlUtilities.CABundlePath;
        public string CurlExecutablePath { get; set; } = CurlUtilities.CurlExecutablePath;

    }

}