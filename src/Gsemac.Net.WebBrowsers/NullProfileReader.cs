using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    internal sealed class NullProfileReader :
        IWebBrowserProfilesReader {

        // Public members

        public IEnumerable<IWebBrowserProfile> GetProfiles() {

            return Enumerable.Empty<IWebBrowserProfile>();

        }

    }

}