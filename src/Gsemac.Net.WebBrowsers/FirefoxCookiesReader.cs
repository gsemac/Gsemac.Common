using Gsemac.Core;
using Gsemac.Net.Http;
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
                            string path = row["path"].ToString();
                            long expiry = (long)row["expiry"];
                            long isSecure = (long)row["isSecure"];
                            long isHttpOnly = (long)row["isHttpOnly"];

                            // Cookie values may already be URL-encoded, or we might have cookies containing unescaped characters (e.g. commas).
                            // The browser stores cookies as they are given, and ideally, we would process them the same way.
                            // Unfortunately, CookieContainer will throw if a cookie value contains a comma or semicolon.

                            // CookieContainer will allow such cookies if we wrap the value in double-quotes or URL-encode the value.
                            // We will use the latter approach, as this is commonly employed by various implementations anyway (e.g. PHP).
                            // However, since cookies may already be URL-encoded, we will only do this if they contain illegal characters.

                            if (!string.IsNullOrWhiteSpace(value) && value.Any(c => HttpUtilities.GetInvalidCookieChars().Contains(c)))
                                value = Uri.EscapeDataString(value);

                            // While cookies with empty names are technically valid and support for them varies by browser,
                            // the constructor for Cookie will throw when given an empty name.

                            if (string.IsNullOrEmpty(name))
                                continue;

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