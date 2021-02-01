using System.Diagnostics;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    public static class WebDriverUtilities {

        public const string ChromeDriverExecutablePath = "chromedriver.exe";
        public const string GeckoDriverExecutablePath = "geckodriver.exe";

        public static void KillWebDriverProcesses(string webDriverExecutablePath) {

            if (string.IsNullOrWhiteSpace(webDriverExecutablePath))
                return;

            webDriverExecutablePath = Path.GetFullPath(webDriverExecutablePath);

            string webDriverProcessName = Path.GetFileNameWithoutExtension(webDriverExecutablePath);

            if (string.IsNullOrWhiteSpace(webDriverProcessName))
                return;

            foreach (Process process in Process.GetProcessesByName(webDriverProcessName)) {

                string processExecutablePath = process.MainModule.FileName;

                if (!string.IsNullOrWhiteSpace(processExecutablePath) && processExecutablePath.Equals(webDriverExecutablePath))
                    process.Kill();

            }

        }

    }

}