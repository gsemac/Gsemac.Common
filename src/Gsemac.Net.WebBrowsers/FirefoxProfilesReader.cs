using Gsemac.Text.Ini;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    public class FirefoxProfilesReader :
        IWebBrowserProfilesReader {

        // Public members

        public FirefoxProfilesReader(string userDataDirectoryPath) {

            if (userDataDirectoryPath is null)
                throw new ArgumentNullException(nameof(userDataDirectoryPath));

            this.userDataDirectoryPath = userDataDirectoryPath;

        }

        public IEnumerable<IWebBrowserProfile> GetProfiles() {

            List<IWebBrowserProfile> profiles = new List<IWebBrowserProfile>();

            // Profile information is stored in the "profiles.ini" file.

            string profilesIniFilePath = Path.Combine(userDataDirectoryPath, "profiles.ini");

            if (File.Exists(profilesIniFilePath)) {

                // Read all profiles from the file.
                // Older profiles will be named "*.default", and newer profiles will be named "*.default-release" (https://superuser.com/a/1556315/1762496).

                IIni profilesIni = IniFactory.Default.FromFile(profilesIniFilePath, new IniOptions {
                    KeyComparer = StringComparer.OrdinalIgnoreCase,
                });

                // Determine which profiles are marked as "default" by iterating through the installs first.

                HashSet<string> defaultProfileDirectoryPaths = new HashSet<string>();

                foreach (IIniSection section in profilesIni.Sections) {

                    if (section.Name.StartsWith("Install", StringComparison.OrdinalIgnoreCase)) {

                        defaultProfileDirectoryPaths.Add(section["Default"]);

                    }
                    else if (section.Name.StartsWith("Profile", StringComparison.OrdinalIgnoreCase)) {

                        string name = section["Name"];
                        string directoryPath = section["Path"];

                        profiles.Add(new WebBrowserProfile(new FirefoxCookiesReader()) {
                            Identifier = section.Name,
                            Name = name,
                            IsDefault = defaultProfileDirectoryPaths.Contains(directoryPath),
                            DirectoryPath = Path.Combine(userDataDirectoryPath, directoryPath),
                        });

                    }

                }

            }

            return profiles.Where(profile => Directory.Exists(profile.DirectoryPath))
                .OrderByDescending(profile => profile.IsDefault)
                .ThenByDescending(profile => profile.Name.EndsWith("-release"));

        }

        // Private members

        private readonly string userDataDirectoryPath;

    }

}