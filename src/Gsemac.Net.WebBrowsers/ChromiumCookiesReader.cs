using Gsemac.Core;
using Gsemac.IO;
using Gsemac.Win32.Native;
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

                UnlockCookiesDatabase(cookiesPath);

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
                            string path = CookieUtilities.SanitizePath(row["path"].ToString());
                            long expiresUtc = (long)row["expires_utc"];
                            long isSecure = (long)row["is_secure"];
                            long isHttpOnly = (long)row["is_httponly"];

                            // While cookies with empty names are technically valid and support for them varies by browser,
                            // the constructor for Cookie will throw when given an empty name.

                            if (string.IsNullOrEmpty(name))
                                continue;

                            if (string.IsNullOrWhiteSpace(value)) {

                                byte[] encryptedValue = (byte[])row["encrypted_value"];
                                byte[] decryptedValue = cookieDecryptor.DecryptCookie(encryptedValue);

                                value = Encoding.UTF8.GetString(decryptedValue);

                            }

                            value = CookieUtilities.SanitizeValue(value);

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

        private static bool UnlockCookiesDatabase(string databaseFilePath) {

            // Since Chrome 114, the cookies database is locked while the browser is open.
            // https://stackoverflow.com/a/76442546

            // We can kill the process locking the database without killing the browser process.
            // The browser will automatically restart the process, so this is transparent to the user.
            // This solution was adapted from the one here: https://github.com/bashonly/yt-dlp-ChromeCookieUnlock/blob/61fd994425e33b9286f8190171922e059181732e/yt_dlp_plugins/postprocessor/chrome_cookie_unlock.py

            // Note that RestartManager API is only available in Windows Vista and later.

            if (string.IsNullOrWhiteSpace(databaseFilePath) || !File.Exists(databaseFilePath))
                return false;

            // If the RestartManager API is not available, we'll assume the database is already unlocked.
            // This would only be applicable to Windows XP and earlier, on which newer versions of Chrome aren't supported anyway.

            if (Environment.OSVersion.Version.Major < 6)
                return true;

            uint sessionFlags = 0;
            StringBuilder sessionKey = new StringBuilder(256);

            string[] fileNames = new[] {
                 databaseFilePath
            };

            if (Rstrtmgr.RmStartSession(out uint sessionHandle, sessionFlags, sessionKey) != Constants.ERROR_SUCCESS)
                return false;

            try {

                RM_REBOOT_REASON rebootReasons = RM_REBOOT_REASON.RmRebootReasonNone;
                RM_PROCESS_INFO[] affectedApps = new RM_PROCESS_INFO[1];
                uint procInfo = (uint)affectedApps.Length;

                if (Rstrtmgr.RmRegisterResources(sessionHandle, 1, fileNames, 0, null, 0, null) != Constants.ERROR_SUCCESS)
                    return false;

                // Get all processes locking the database file.

                int getListResult = Rstrtmgr.RmGetList(sessionHandle, out uint procInfoNeeded, ref procInfo, affectedApps, out rebootReasons);

                // The process info array wasn't large enough to store the process information.

                if (getListResult == Constants.ERROR_MORE_DATA) {

                    affectedApps = new RM_PROCESS_INFO[procInfoNeeded];
                    procInfo = (uint)affectedApps.Length;

                    getListResult = Rstrtmgr.RmGetList(sessionHandle, out procInfoNeeded, ref procInfo, affectedApps, out rebootReasons);

                }

                if (getListResult != Constants.ERROR_SUCCESS)
                    return false;

                // If there were no processes locking the database, there's nothing we need to do.

                if (procInfoNeeded <= 0)
                    return true;

                // Terminate the processes locking the database file.

                if (Rstrtmgr.RmShutdown(sessionHandle, RM_SHUTDOWN_TYPE.RmForceShutdown, null) != Constants.ERROR_SUCCESS)
                    return false;

                // The database file was successfully unlocked.

                return true;

            }
            finally {

                Rstrtmgr.RmEndSession(sessionHandle);

            }

        }

    }

}