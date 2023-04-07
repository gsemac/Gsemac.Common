using System;
using System.Collections.Generic;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    internal class BrowserProfile :
        IBrowserProfile {

        // Public members

        public string Name { get; set; }
        public bool IsDefaultProfile { get; set; }
        public string DirectoryPath { get; set; }

        public BrowserProfile(IBrowserCookiesReader cookiesReader) {

            if (cookiesReader is null)
                throw new ArgumentNullException(nameof(cookiesReader));

            this.cookiesReader = cookiesReader;

        }

        public IEnumerable<Cookie> GetCookies() {

            return cookiesReader.GetCookies(this);

        }

        // Private members

        private readonly IBrowserCookiesReader cookiesReader;

    }

}