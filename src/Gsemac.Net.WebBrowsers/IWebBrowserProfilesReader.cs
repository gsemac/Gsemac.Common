using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public interface IWebBrowserProfilesReader {

        IEnumerable<IWebBrowserProfile> GetProfiles();

    }

}