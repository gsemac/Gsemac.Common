namespace Gsemac.Net.WebBrowsers {

    public class CookiesReaderFactoryOptions :
        ICookiesReaderFactoryOptions {

        public string ProfileDirectory { get; set; }

        public static CookiesReaderFactoryOptions Default => new CookiesReaderFactoryOptions();

    }

}