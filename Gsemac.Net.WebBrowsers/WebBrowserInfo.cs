using Gsemac.Utilities;
using System;
using System.Diagnostics;
using System.Text;

namespace Gsemac.Net.WebBrowsers {

    public class WebBrowserInfo :
        IWebBrowserInfo {

        // Public members

        public string Name { get; }
        public Version Version { get; }
        public string ExecutablePath { get; }
        public bool Is64BitExecutable { get; } = false;

        public WebBrowserInfo(string browserExecutablePath) {

            this.ExecutablePath = browserExecutablePath;

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(browserExecutablePath);

            this.Name = versionInfo.ProductName;
            this.Version = new Version(versionInfo.ProductVersion);
            this.Is64BitExecutable = PathUtilities.PathContainsSegment(browserExecutablePath, "Program Files (x86)");

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append($"{Name}");

            if (Is64BitExecutable)
                sb.Append(" (64-bit)");

            return sb.ToString();

        }

    }

}