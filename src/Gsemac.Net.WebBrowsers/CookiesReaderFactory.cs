using System;

namespace Gsemac.Net.WebBrowsers {

    public class CookiesReaderFactory :
        ICookiesReaderFactory {

        // Public members

        public CookiesReaderFactory() :
            this(CookiesReaderFactoryOptions.Default) {
        }
        public CookiesReaderFactory(ICookiesReaderFactoryOptions options) {

            this.options = options;

        }

        public ICookiesReader Create(IWebBrowserInfo webBrowserInfo) {

            switch (webBrowserInfo.Id) {

                case WebBrowserId.Chrome:
                    return CreateChromeCookiesReader();

                case WebBrowserId.Firefox:
                    return CreateFirefoxCookiesReader();

                default:
                    throw new ArgumentOutOfRangeException(nameof(webBrowserInfo));

            }

        }

        // Private members

        private readonly ICookiesReaderFactoryOptions options;

        private ICookiesReader CreateChromeCookiesReader() {

            ChromeCookiesReader cookiesReader = new ChromeCookiesReader();

            if (!string.IsNullOrWhiteSpace(options.ProfileDirectory))
                cookiesReader.ProfileDirectory = options.ProfileDirectory;

            return cookiesReader;

        }
        private ICookiesReader CreateFirefoxCookiesReader() {

            return new FirefoxCookiesReader();

        }

    }

}