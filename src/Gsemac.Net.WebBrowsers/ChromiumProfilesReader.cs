using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    public class ChromiumProfilesReader :
        IWebBrowserProfilesReader {

        // Public members

        public ChromiumProfilesReader(string userDataDirectoryPath) {

            if (userDataDirectoryPath is null)
                throw new ArgumentNullException(nameof(userDataDirectoryPath));

            this.userDataDirectoryPath = userDataDirectoryPath;

        }

        public IEnumerable<IWebBrowserProfile> GetProfiles() {

            List<IWebBrowserProfile> profiles = new List<IWebBrowserProfile>();

            // Profile information is stored in the "Local State" file as JSON.

            string localStateFilePath = Path.Combine(userDataDirectoryPath, "Local State");

            if (File.Exists(localStateFilePath)) {

                // Read all profiles from the file.

                // The "profile.last_used" key may not be present (it wasn't present for Edge).
                // If there's only one profile, we'll just assume that it's the default.

                JObject localState = JObject.Parse(File.ReadAllText(localStateFilePath));
                JToken infoCacheToken = localState["profile"]?["info_cache"];
                string lastUsedProfileIdentifier = localState["profile"]?["last_used"]?.Value<string>() ?? string.Empty;

                if (infoCacheToken is object) {

                    IEnumerable<JProperty> profileTokens = infoCacheToken.Children().OfType<JProperty>();

                    foreach (JProperty profileToken in profileTokens) {

                        string identifier = profileToken.Name ?? string.Empty;
                        string name = profileToken.SelectToken("..name")?.Value<string>() ?? string.Empty;
                        string directoryPath = Path.Combine(userDataDirectoryPath, identifier);

                        profiles.Add(new WebBrowserProfile(new ChromiumCookiesReader()) {
                            Identifier = identifier,
                            Name = name,
                            IsDefault = identifier.Equals(lastUsedProfileIdentifier) || profileTokens.Count() == 1,
                            DirectoryPath = directoryPath,
                        });

                    }

                }

            }

            return profiles.Where(profile => Directory.Exists(profile.DirectoryPath))
                .OrderByDescending(profile => profile.IsDefault);

        }

        // Private members

        private readonly string userDataDirectoryPath;


    }

}