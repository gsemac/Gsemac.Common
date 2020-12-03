using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gsemac.Net.Extensions;
using Gsemac.Threading.Tasks;

namespace Gsemac.Net.WebBrowsers {

    public static class WebBrowserUtilities {

        // Public members

        public static CookieContainer GetWebBrowserCookies(IWebBrowserInfo webBrowserInfo) {

            switch (webBrowserInfo.Id) {

                case WebBrowserId.GoogleChrome:
                    return GetChromeWebBrowserCookies();

                default:
                    throw new ArgumentException(nameof(webBrowserInfo));

            }

        }

        public static WebHeaderCollection GetWebBrowserRequestHeaders(IWebBrowserInfo webBrowserInfo) {

            return GetWebBrowserRequestHeaders(webBrowserInfo, TimeSpan.FromSeconds(10));

        }
        public static WebHeaderCollection GetWebBrowserRequestHeaders(IWebBrowserInfo webBrowserInfo, TimeSpan timeout) {

            return GetWebBrowserRequestHeaders(webBrowserInfo, timeout, null);

        }
        public static WebHeaderCollection GetWebBrowserRequestHeaders(IWebBrowserInfo webBrowserInfo, TimeSpan timeout, string responseBody) {

            WebHeaderCollection requestHeaders = new WebHeaderCollection();

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            if (!File.Exists(webBrowserInfo?.ExecutablePath))
                throw new FileNotFoundException("Cannot locate then web browser.");

            // Start an HTTP server on a random port.

            Uri uri = new Uri($"http://localhost:{SocketUtilities.GetUnusedPort()}/");
            bool listenForRequests = true;

            using (HttpListener listener = new HttpListener()) {

                listener.Prefixes.Add(uri.AbsoluteUri);

                listener.Start();

                // Open the HTTP server in the user's web browser.

                Uri requestUri = new Uri(uri, Path.Combine(Assembly.GetExecutingAssembly().GetName().Name, nameof(GetWebBrowserRequestHeaders)));

                Process.Start(webBrowserInfo.ExecutablePath, requestUri.AbsoluteUri);

                // Wait for an incoming request.

                while (listenForRequests) {

                    listenForRequests = false;

                    HttpListenerContext context = null;

                    IAsyncResult asyncResult = listener.BeginGetContext(_ => { }, null);

                    Task.WaitAny(Task.Factory.StartNew(() => context = listener.EndGetContext(asyncResult)), TaskUtilities.Delay(timeout.TotalMilliseconds));

                    if (!(context is null)) {

                        HttpListenerRequest request = context.Request;

                        if (request.HttpMethod.Equals("get", StringComparison.OrdinalIgnoreCase)) {

                            // The web browser requested our webpage, so copy the request headers.

                            context.Request.Headers.CopyTo(requestHeaders);

                            // Respond to the request.

                            HttpListenerResponse response = context.Response;

                            StringBuilder responseBuilder = new StringBuilder();

                            responseBuilder.AppendLine("<!DOCTYPE html>");
                            responseBuilder.Append("<html>");
                            responseBuilder.Append("<body>");
                            responseBuilder.Append("<script>fetch(\"" + requestUri.AbsoluteUri + "\", {method: \"POST\"});</script>");
                            responseBuilder.Append(responseBody ?? "This window may now be closed.");
                            responseBuilder.Append("</body>");
                            responseBuilder.Append("</html>");

                            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(responseBuilder.ToString())))
                                ms.CopyTo(response.OutputStream);

                            response.Close();

                            // Wait for the web browser to POST back let us know it got our response.

                            listenForRequests = true;

                        }
                        else {

                            // The web browser has POSTed back to let us know it got our response.

                        }

                    }

                }

            }

            return requestHeaders;

        }

        // Private members

        private static string GetChromeCookiesPath() {

            string chromeCookiesPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
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