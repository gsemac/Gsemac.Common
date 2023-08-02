using System;
using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public static class WebBrowserInfoFactoryExtensions {

        public static IWebBrowserInfo GetWebBrowserInfo(this IWebBrowserInfoFactory webBrowserInfoFactory, WebBrowserId webBrowserId) {

            if (webBrowserInfoFactory is null)
                throw new ArgumentNullException(nameof(webBrowserInfoFactory));

            return webBrowserInfoFactory.GetWebBrowserInfo(webBrowserId, WebBrowserInfoOptions.Default);

        }

        public static IWebBrowserInfo GetDefaultWebBrowser(this IWebBrowserInfoFactory webBrowserInfoFactory) {

            if (webBrowserInfoFactory is null)
                throw new ArgumentNullException(nameof(webBrowserInfoFactory));

            return webBrowserInfoFactory.GetDefaultWebBrowser(WebBrowserInfoOptions.Default);

        }
        public static IEnumerable<IWebBrowserInfo> GetInstalledWebBrowsers(this IWebBrowserInfoFactory webBrowserInfoFactory) {

            if (webBrowserInfoFactory is null)
                throw new ArgumentNullException(nameof(webBrowserInfoFactory));

            return webBrowserInfoFactory.GetInstalledWebBrowsers(WebBrowserInfoOptions.Default);

        }

    }

}