using Gsemac.Text;
using System;
using System.IO;
using System.Text;

namespace Gsemac.Net.WebBrowsers {

    internal static class FirefoxUtilities {

        // Public members

        public static void CreateFirefoxUserDataDirectory(string userDataDirectoryPath) {

            if (userDataDirectoryPath is null)
                throw new ArgumentNullException(nameof(userDataDirectoryPath));

            string profilesIniPath = Path.Combine(userDataDirectoryPath, "profiles.ini");

            if (!Directory.Exists(userDataDirectoryPath))
                Directory.CreateDirectory(userDataDirectoryPath);

            if (!File.Exists(profilesIniPath)) {

                // Create a default profile.

                string installName = GenerateFirefoxInstallName();
                string profileName = GenerateFirefoxProfileName();

                StringBuilder profilesIniBuilder = new StringBuilder();

                profilesIniBuilder.AppendLine($"[Install{installName}]");
                profilesIniBuilder.AppendLine($"Default=Profiles/{profileName}");
                profilesIniBuilder.AppendLine($"Locked=1");
                profilesIniBuilder.AppendLine();
                profilesIniBuilder.AppendLine($"[Profile0]");
                profilesIniBuilder.AppendLine($"Name=default");
                profilesIniBuilder.AppendLine($"IsRelative=1");
                profilesIniBuilder.AppendLine($"Path=Profiles/{profileName}");
                profilesIniBuilder.AppendLine($"Default=1");

                File.WriteAllText(profilesIniPath, profilesIniBuilder.ToString());

                Directory.CreateDirectory(Path.Combine(userDataDirectoryPath, "Profiles", profileName));

            }

        }

        // Private members

        private static string GenerateFirefoxInstallName() {

            // Installation names are 16-character hexadecimal strings.

            string alphabet = "1234567890ABCDEF";

            return StringUtilities.GetRandomString(alphabet, 16);

        }
        private static string GenerateFirefoxProfileName() {

            string alphabet = "abcdefghijklmnopqrztuvwxyz1234567890";

            return $"{StringUtilities.GetRandomString(alphabet, 8).ToLowerInvariant()}.default";

        }

    }

}