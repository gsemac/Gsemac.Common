using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Text;

namespace Gsemac.Net.WebBrowsers {

    public class ChromeWebBrowserCookieReader :
        IWebBrowserCookieReader {

        // Public members

        public CookieContainer GetCookies() {

            return GetChromeWebBrowserCookies();

        }
        public CookieCollection GetCookies(Uri uri) => GetCookies().GetCookies(uri);

        // Private members

        private static string GetChromeCookiesPath() {

            string chromeCookiesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"Google\Chrome\User Data\Default\Cookies");

            if (File.Exists(chromeCookiesPath))
                return chromeCookiesPath;

            throw new FileNotFoundException("Could not determine cookies path.", chromeCookiesPath);

        }
        private static CookieContainer GetChromeWebBrowserCookies() {

            CookieContainer cookies = new CookieContainer();
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

                        if (string.IsNullOrWhiteSpace(value)) {

                            byte[] encryptedValue = (byte[])row["encrypted_value"];
                            byte[] decryptedValue = cookieDecryptor.DecryptCookie(encryptedValue);

                            value = Encoding.UTF8.GetString(decryptedValue);

                        }

                        // Chrome doesn't escape cookies before saving them.

                        value = Uri.EscapeDataString(value?.Trim());

                        cookies.Add(new Cookie(name, value, path, domain));

                    }

                }

            }

            return cookies;

        }

    }

}