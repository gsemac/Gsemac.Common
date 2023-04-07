using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    internal sealed class NullBrowserProfileReader :
        IBrowserProfilesReader {

        // Public members

        public IEnumerable<IBrowserProfile> GetProfiles() {

            return Enumerable.Empty<IBrowserProfile>();

        }

    }

}