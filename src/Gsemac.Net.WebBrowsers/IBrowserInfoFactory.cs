using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public interface IBrowserInfoFactory {

        IBrowserInfo GetBrowserInfo(string browserExecutablePath);
        IBrowserInfo GetBrowserInfo(BrowserId browserId, IBrowserInfoOptions options);

        IBrowserInfo GetDefaultBrowser(IBrowserInfoOptions options);
        IEnumerable<IBrowserInfo> GetInstalledBrowsers(IBrowserInfoOptions options);

    }

}