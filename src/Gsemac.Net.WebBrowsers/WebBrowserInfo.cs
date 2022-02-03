using System.Text;

namespace Gsemac.Net.WebBrowsers {

    internal class WebBrowserInfo :
        IWebBrowserInfo {

        // Public members

        public string Name { get; set; }
        public System.Version Version { get; set; }
        public string ExecutablePath { get; set; }
        public bool Is64Bit { get; set; }
        public WebBrowserId Id { get; set; }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append($"{Name} {Version}");

            if (Is64Bit)
                sb.Append(" (64-bit)");

            return sb.ToString();

        }

    }

}