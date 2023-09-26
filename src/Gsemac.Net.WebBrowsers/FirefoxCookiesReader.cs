using Gsemac.Core;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    internal class FirefoxCookiesReader :
        IWebBrowserCookiesReader {

        // Public members

        public CookieContainer GetCookies(IWebBrowserProfile profile) {

            if (profile is null)
                throw new ArgumentNullException(nameof(profile));

            string cookiesFilePath = GetCookiesFilePath(profile);

            CookieContainer cookies = new CookieContainer();

            if (File.Exists(cookiesFilePath)) {

                // Firefox stores its cookies in an SQLite database.

                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={cookiesFilePath}")) {
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM moz_cookies", conn))
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    using (DataTable dt = new DataTable()) {

                        adapter.Fill(dt);

                        foreach (DataRow row in dt.Rows) {

                            string name = row["name"].ToString();
                            string value = row["value"].ToString();
                            string domain = row["host"].ToString();
                            string path = CookieUtilities.SanitizePath(row["path"].ToString());
                            long expiry = (long)row["expiry"];
                            long isSecure = (long)row["isSecure"];
                            long isHttpOnly = (long)row["isHttpOnly"];

                            // While cookies with empty names are technically valid and support for them varies by browser,
                            // the constructor for Cookie will throw when given an empty name.

                            if (string.IsNullOrEmpty(name))
                                continue;

                            value = CookieUtilities.SanitizeValue(value);

                            Cookie cookie = new Cookie(name, value, path, domain) {
                                Secure = isSecure > 0,
                                HttpOnly = isHttpOnly > 0,
                            };

                            if (expiry > 0)
                                cookie.Expires = TimestampToDateTimeUtc(expiry);

                            cookies.Add(cookie);

                        }

                    }

                }

            }

            return cookies;

        }

        // Private members

        private string GetCookiesFilePath(IWebBrowserProfile profile) {

            if (profile is null)
                throw new ArgumentNullException(nameof(profile));

            if (!Directory.Exists(profile.DirectoryPath))
                return string.Empty;

            // Cookies are stored in a "cookies.sqlite" file at the root of the profile directory.

            return new[] {
                Path.Combine(profile.DirectoryPath,"cookies.sqlite"),
            }.Where(File.Exists)
            .FirstOrDefault();

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