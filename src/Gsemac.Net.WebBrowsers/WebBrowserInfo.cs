using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Net.WebBrowsers {

    internal class WebBrowserInfo :
        IWebBrowserInfo {

        // Public members

        public string Name { get; set; }
        public Version Version { get; set; }
        public string ExecutablePath { get; set; }
        public string UserDataDirectoryPath { get; set; }
        public bool Is64Bit { get; set; }
        public bool IsDefault { get; set; }
        public WebBrowserId Id { get; set; }

        public WebBrowserInfo() :
            this(new NullProfileReader()) {
        }
        public WebBrowserInfo(IWebBrowserProfilesReader profilesReader) {

            if (profilesReader is null)
                throw new ArgumentNullException(nameof(profilesReader));

            this.profilesReader = profilesReader;

        }

        public IEnumerable<IWebBrowserProfile> GetProfiles() {

            return profilesReader.GetProfiles();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append($"{Name} {Version}");

            if (Is64Bit)
                sb.Append(" (64-bit)");

            return sb.ToString();

        }

        // Private members

        private readonly IWebBrowserProfilesReader profilesReader;

    }

}