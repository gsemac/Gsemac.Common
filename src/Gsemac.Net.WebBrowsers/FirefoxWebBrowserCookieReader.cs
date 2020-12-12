using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    public class FirefoxWebBrowserCookieReader :
        IWebBrowserCookieReader {

        // Public members

        public CookieContainer GetCookies() {

            return GetFirefoxWebBrowserCookies();

        }
        public CookieCollection GetCookies(Uri uri) => GetCookies().GetCookies(uri);

        // Private members

        private static string GetFirefoxCookiesPath() {

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
        private static CookieContainer GetFirefoxWebBrowserCookies() {

            CookieContainer cookies = new CookieContainer();
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

                        cookies.Add(new Cookie(name, value, path, domain));

                    }

                }

            }

            return cookies;

        }

    }

}