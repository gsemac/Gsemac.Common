using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public interface IBrowserProfilesReader {

        IEnumerable<IBrowserProfile> GetProfiles();

    }

}