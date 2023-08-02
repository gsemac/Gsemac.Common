using System;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    internal class WebBrowserProfile :
        IWebBrowserProfile {

        // Public members

        public string Identifier { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string DirectoryPath { get; set; }

        public WebBrowserProfile(IWebBrowserCookiesReader cookiesReader) {

            if (cookiesReader is null)
                throw new ArgumentNullException(nameof(cookiesReader));

            this.cookiesReader = cookiesReader;

        }

        public CookieContainer GetCookies() {

            return cookiesReader.GetCookies(this);

        }

        // Private members

        private readonly IWebBrowserCookiesReader cookiesReader;

    }

}