using Gsemac.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Gsemac.Net.WebBrowsers.Extensions {

    public static class CookiesReaderExtensions {

        public static IEnumerable<Cookie> GetCookies(this ICookiesReader cookiesReader, string uri) {

            return cookiesReader.GetCookies()
                .Where(c => new CookieDomainPattern(c.Domain).IsMatch(uri));

        }
        public static IEnumerable<Cookie> GetCookies(this ICookiesReader cookiesReader, Uri uri) {

            return GetCookies(cookiesReader, uri.AbsoluteUri);

        }

    }

}
