﻿using Gsemac.Core;
using Gsemac.IO;
using Gsemac.Net.Http;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Gsemac.Net.WebBrowsers {

    internal class ChromiumCookiesReader :
        IWebBrowserCookiesReader {

        // Public members

        public CookieContainer GetCookies(IWebBrowserProfile profile) {

            if (profile is null)
                throw new ArgumentNullException(nameof(profile));

            string cookiesPath = GetCookiesFilePath(profile);

            CookieContainer cookies = new CookieContainer();

            if (File.Exists(cookiesPath)) {

                // Chrome stores its cookies in an SQLite database.

                IWebBrowserCookieDecryptor cookieDecryptor = new ChromiumCookieDecryptor(PathUtilities.GetParentPath(profile.DirectoryPath));

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

                            if (expiresUtc > 0)
                                cookie.Expires = TimestampToDateTimeUtc(expiresUtc);

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

            // Cookies used to be stored in a "Cookies" file at the root of the profile directory.
            // Cookies are now stored in "./Network/Cookies" as of Chrome 96 (https://stackoverflow.com/q/31021764/5383169).

            return new[] {
                Path.Combine(profile.DirectoryPath, "Network", "Cookies"),
                Path.Combine(profile.DirectoryPath, "Cookies"),
            }.Where(File.Exists)
            .FirstOrDefault();

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