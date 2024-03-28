using System;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    public static class WebBrowserProfilesReaderExtensions {

        // Public members

        public static IWebBrowserProfile GetDefaultProfile(this IWebBrowserProfilesReader profilesReader) {

            if (profilesReader is null)
                throw new ArgumentNullException(nameof(profilesReader));

            return profilesReader.GetProfiles()
                .Where(profile => profile.IsDefault)
                .FirstOrDefault();

        }

    }

}
