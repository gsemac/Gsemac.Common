using System;
using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public static class BrowserInfoFactoryExtensions {

        public static IBrowserInfo GetBrowserInfo(this IBrowserInfoFactory webBrowserInfoFactory, BrowserId webBrowserId) {

            if (webBrowserInfoFactory is null)
                throw new ArgumentNullException(nameof(webBrowserInfoFactory));

            return webBrowserInfoFactory.GetBrowserInfo(webBrowserId, BrowserInfoOptions.Default);

        }
        public static IEnumerable<IBrowserInfo> GetInstalledBrowsers(this IBrowserInfoFactory webBrowserInfoFactory) {

            if (webBrowserInfoFactory is null)
                throw new ArgumentNullException(nameof(webBrowserInfoFactory));

            return webBrowserInfoFactory.GetInstalledBrowsers(BrowserInfoOptions.Default);

        }

    }

}