using Gsemac.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.WebBrowsers {

    public class ChromeCookiesReader :
        ICookiesReader {

        // Public members

        public string ProfileDirectory { get; set; }

        public IEnumerable<Cookie> GetCookies() {

            return GetChromeCookies();

        }

        // Private members

        private string GetChromeCookiesPath() {

            string chromeCookiesPath = GetProfileDirectoryPaths().Select(path => Path.Combine(path, "Cookies"))
                .Where(path => File.Exists(path))
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(chromeCookiesPath))
                throw new FileNotFoundException("Could not determine cookies path.", chromeCookiesPath);

            return chromeCookiesPath;

        }
        private IEnumerable<Cookie> GetChromeCookies() {

            string cookiesPath = GetChromeCookiesPath();
            ICookieDecryptor cookieDecryptor = new ChromeCookieDecryptor();

            // Chrome stores its cookies in an SQLite database.

            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={cookiesPath}")) {
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Cookies", conn))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                using (DataTable dt = new DataTable()) {

                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows) {

                        string name = row["name"].ToString();
                        string value = row["value"].ToString();
                        string domain = row["host_key"].ToString();
                        string path = row["path"].ToString();
                        long expiresUtc = (long)row["expires_utc"];
                        long isSecure = (long)row["is_secure"];
                        long isHttpOnly = (long)row["is_httponly"];

                        if (string.IsNullOrWhiteSpace(value)) {

                            byte[] encryptedValue = (byte[])row["encrypted_value"];
                            byte[] decryptedValue = cookieDecryptor.DecryptCookie(encryptedValue);

                            value = Encoding.UTF8.GetString(decryptedValue);

                        }

                        // Chrome doesn't escape cookies before saving them.

                        value = Uri.EscapeDataString(value?.Trim());

                        Cookie cookie = new Cookie(name, value, path, domain) {
                            Secure = isSecure > 0,
                            HttpOnly = isHttpOnly > 0,
                        };

                        if (expiresUtc > 0)
                            cookie.Expires = TimestampToDateTimeUtc(expiresUtc);

                        yield return cookie;

                    }

                }

            }

        }
        private string GetUserDataDirectoryPath() {

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                $@"Google\Chrome\User Data\");

        }
        private string GetLastActiveProfileDirectoryPath() {

            // Get the path to the last profile used by the user according to the information in the "Local State" file.

            string localStateFilePath = Path.Combine(GetUserDataDirectoryPath(), "Local State");
            string lastActiveProfileName = string.Empty;

            if (File.Exists(localStateFilePath)) {

                JObject localState = JObject.Parse(File.ReadAllText(localStateFilePath));
                JToken infoCacheNode = localState["profile"]["info_cache"];

                lastActiveProfileName = infoCacheNode?.Children()
                    .OfType<JProperty>()
                    .OrderByDescending(prop => prop.Children()["active_time"]?.FirstOrDefault()?.Value<double>())
                    .FirstOrDefault()?
                    .Name;

            }

            if (!string.IsNullOrWhiteSpace(lastActiveProfileName))
                return Path.Combine(GetUserDataDirectoryPath(), lastActiveProfileName);

            return lastActiveProfileName;

        }
        private IEnumerable<string> GetProfileDirectoryPaths() {

            string userDataDirectoryPath = GetUserDataDirectoryPath();

            List<Lazy<string>> profilePaths = new List<Lazy<string>> {
               new Lazy<string>(() => Path.Combine(userDataDirectoryPath, string.IsNullOrWhiteSpace(ProfileDirectory) ? "Default" : ProfileDirectory)),
               new Lazy<string>(() => GetLastActiveProfileDirectoryPath()),
            };

            // For some users, a "Default" profile is not present.
            // Add all profiles we can find to the list of profiles.

            if (Directory.Exists(userDataDirectoryPath)) {

                foreach (string directoryPath in Directory.GetDirectories(userDataDirectoryPath, "*", SearchOption.TopDirectoryOnly)) {

                    if (Regex.IsMatch(new DirectoryInfo(directoryPath).Name, @"^Profile\s\d+$"))
                        profilePaths.Add(new Lazy<string>(() => directoryPath));

                }

            }

            return profilePaths.Select(lazy => lazy.Value)
                .Where(path => Directory.Exists(path));

        }
        private DateTime TimestampToDateTimeUtc(long timestamp) {

            // CHrome's epoch starts at 1601-01-01T00:00:00Z, and the timestamp is in nanoseconds:
            // https://stackoverflow.com/a/43520042

            long timestampSeconds = timestamp / 1000000;

            timestampSeconds -= 11644473600; // 1601-01-01T00:00:00Z is 11644473600 seconds before the unix epoch

            // DateTime is not capable of representing the same range of dates as the browser's timestamps.

            if (timestampSeconds > DateUtilities.ToUnixTimeSeconds(DateTimeOffset.MaxValue))
                return DateTime.MaxValue;

            return DateUtilities.FromUnixTimeSeconds(timestampSeconds).DateTime;

        }

    }

}