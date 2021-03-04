using Gsemac.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    public class FirefoxCookiesReader :
        ICookiesReader {

        // Public members

        public IEnumerable<Cookie> GetCookies() {

            return GetFirefoxCookies();

        }

        // Private members

        private string GetFirefoxCookiesPath() {

            string firefoxProfilesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"Mozilla\Firefox\Profiles\");

            if (Directory.Exists(firefoxProfilesDirectory)) {

                // Older profiles will be named "*.default", and newer profiles will be named "*.default-release".

                string firefoxProfileDirectory = Directory.GetDirectories(firefoxProfilesDirectory, "*.default-release", SearchOption.TopDirectoryOnly)
                    .Concat(Directory.GetDirectories(firefoxProfilesDirectory, "*.default", SearchOption.TopDirectoryOnly))
                    .FirstOrDefault();

                if (Directory.Exists(firefoxProfileDirectory)) {

                    string firefoxCookiesPath = Path.Combine(firefoxProfileDirectory, "cookies.sqlite");

                    if (File.Exists(firefoxCookiesPath))
                        return firefoxCookiesPath;

                }

            }

            throw new FileNotFoundException("Could not determine cookies path.");

        }
        private IEnumerable<Cookie> GetFirefoxCookies() {

            string cookiesPath = GetFirefoxCookiesPath();

            // Firefox stores its cookies in an SQLite database.

            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={cookiesPath}")) {
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM moz_cookies", conn))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                using (DataTable dt = new DataTable()) {

                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows) {

                        string name = row["name"].ToString();
                        string value = row["value"].ToString();
                        string domain = row["host"].ToString();
                        string path = row["path"].ToString();
                        long expiry = (long)row["expiry"];
                        long isSecure = (long)row["isSecure"];
                        long isHttpOnly = (long)row["isHttpOnly"];

                        Cookie cookie = new Cookie(name, value, path, domain) {
                            Secure = isSecure > 0,
                            HttpOnly = isHttpOnly > 0,
                        };

                        if (expiry > 0)
                            cookie.Expires = TimestampToDateTimeUtc(expiry);

                        yield return cookie;

                    }

                }

            }

        }
        private DateTime TimestampToDateTimeUtc(long timestamp) {

            // Firefox stores expiry timestamps as unix time seconds.

            // DateTime is not capable of representing the same range of dates as the browser's timestamps.

            if (timestamp > DateUtilities.ToUnixTimeSeconds(DateTimeOffset.MaxValue))
                return DateTime.MaxValue;

            return DateUtilities.FromUnixTimeSeconds(timestamp).DateTime;

        }

    }

}