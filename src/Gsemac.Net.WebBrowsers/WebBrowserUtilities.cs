using Gsemac.IO;
using Gsemac.Net.Extensions;
using Gsemac.Net.Http.Extensions;
using Gsemac.Net.Sockets;
using Gsemac.Net.WebBrowsers.Properties;
using Gsemac.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.WebBrowsers {

    public static class WebBrowserUtilities {

        // Public members

        public static WebHeaderCollection GetBrowserRequestHeaders(IWebBrowserInfo webBrowserInfo) {

            return GetBrowserRequestHeaders(webBrowserInfo, TimeSpan.FromSeconds(10));

        }
        public static WebHeaderCollection GetBrowserRequestHeaders(IWebBrowserInfo webBrowserInfo, TimeSpan timeout) {

            return GetBrowserRequestHeaders(webBrowserInfo, timeout, null);

        }
        public static WebHeaderCollection GetBrowserRequestHeaders(IWebBrowserInfo webBrowserInfo, TimeSpan timeout, string responseBody) {

            WebHeaderCollection requestHeaders = new WebHeaderCollection();

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            if (!File.Exists(webBrowserInfo?.ExecutablePath))
                throw new FileNotFoundException();

            // Start an HTTP server on a random port.

            Uri requestUri = new Uri($"http://localhost:{SocketUtilities.GetAvailablePort()}/");

            using (HttpListener listener = new HttpListener()) {

                listener.Prefixes.Add(requestUri.AbsoluteUri);

                listener.Start();

                // Open the HTTP server in the user's web browser.

                Process.Start(webBrowserInfo.ExecutablePath, requestUri.AbsoluteUri);

                // Wait for an incoming request.

                HttpListenerContext context = listener.GetContext(timeout);

                if (context is object) {

                    HttpListenerRequest request = context.Request;

                    if (request.HttpMethod.Equals("get", StringComparison.OrdinalIgnoreCase)) {

                        // The web browser requested our webpage, so copy the request headers.

                        context.Request.Headers.CopyTo(requestHeaders);

                        // Respond to the request.

                        using (HttpListenerResponse response = context.Response) {

                            StringBuilder responseBuilder = new StringBuilder();

                            responseBuilder.AppendLine("<!DOCTYPE html>");
                            responseBuilder.Append("<html>");
                            responseBuilder.Append("<body>");
                            responseBuilder.Append(responseBody ?? "This page may now be closed.");
                            responseBuilder.Append("</body>");
                            responseBuilder.Append("</html>");

                            byte[] responseBytes = Encoding.UTF8.GetBytes(responseBuilder.ToString());

                            // We need to be careful with how we handle our resources to ensure that HttpListener finishes responding before stopping.
                            // While calling "Close()" on the output stream may look redundant, it seems to help.
                            //m https://stackoverflow.com/a/72939272/5383169

                            response.ContentLength64 = responseBytes.Length;
                            response.KeepAlive = true;

                            using (MemoryStream contentStream = new MemoryStream(responseBytes))
                            using (Stream outputStream = response.OutputStream) {

                                contentStream.CopyTo(response.OutputStream);

                                outputStream.Close();

                            }

                            response.Close();

                        }

                        listener.Stop();

                    }

                }

            }

            return requestHeaders;

        }

        public static string EscapeUriString(string stringToEscape) {

            return EscapeUriString(stringToEscape, WebBrowserId.Chrome);

        }
        public static string EscapeUriString(string stringToEscape, WebBrowserId webBrowserId) {

            if (string.IsNullOrEmpty(stringToEscape))
                return stringToEscape;

            // Web browsers escape URLs differently compared to Uri.EscapeUriString.
            // Certain reserved characters are never escaped, while others are escaped only contextually.
            // Each browser has its own specific behavior.

            // Non-ASCII characters are always escaped.

            char[] globallyRestrictedChars = new[] { ' ', '<', '>', '"' };
            char[] pathRestrictedChars = null;
            char[] queryRestrictedChars = new[] { '\'' };

            if (webBrowserId != WebBrowserId.Firefox) {

                // The following applies to all Chromium-based browsers.

                pathRestrictedChars = new[] { '|' };

            }

            stringToEscape = PercentEncodeChars(stringToEscape, globallyRestrictedChars, encodeNonAsciiChars: true);

            stringToEscape = Regex.Replace(stringToEscape, @"^(?<scheme>[^:]+:\/\/)(?<authority>[^\/#]+)(?<path>\/[^#?]*)?(?<query>\?[^#]*)?(?<fragment>#.+)?$", m => {

                StringBuilder resultBuilder = new StringBuilder();

                resultBuilder.Append(m.Groups["scheme"].Value);
                resultBuilder.Append(m.Groups["authority"].Value);
                resultBuilder.Append(PercentEncodeChars(m.Groups["path"].Value, pathRestrictedChars, false));
                resultBuilder.Append(PercentEncodeChars(m.Groups["query"].Value, queryRestrictedChars, false));
                resultBuilder.Append(m.Groups["fragment"].Value);

                return resultBuilder.ToString();

            });

            return stringToEscape;

        }

        public static bool OpenUrl(Uri uri) {

            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            return OpenUrl(uri, OpenUrlOptions.Default);

        }
        public static bool OpenUrl(Uri uri, IOpenUrlOptions options) {

            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return OpenUrl(uri.AbsoluteUri, options);

        }
        public static bool OpenUrl(string url) {

            if (url is null)
                throw new ArgumentNullException(nameof(url));

            return OpenUrl(url, OpenUrlOptions.Default);

        }
        public static bool OpenUrl(string url, IOpenUrlOptions options) {

            if (url is null)
                throw new ArgumentNullException(nameof(url));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (!PathUtilities.IsUrl(url))
                throw new ArgumentException(ExceptionMessages.StringIsNotAValidUrl, nameof(url));

            IWebBrowserInfo browserInfo = options.WebBrowser;
            IWebBrowserProfile browserProfile = options.Profile;

            if (browserInfo is null && options.WebBrowserId != WebBrowserId.Unknown)
                browserInfo = WebBrowserInfoFactory.Default.GetWebBrowserInfo(options.WebBrowserId);

            string browserExecutablePath = browserInfo?.ExecutablePath ?? "explorer.exe";

            List<string> arguments = new List<string> {
                $"\"{url}\""
            };

            if (browserInfo is object) {

                switch (browserInfo.Id) {

                    case WebBrowserId.Chrome:
                    case WebBrowserId.Edge:

                        if (!string.IsNullOrWhiteSpace(options.UserDataDirectoryPath))
                            arguments.Add($"--user-data-dir=\"{options.UserDataDirectoryPath}\"");

                        if (browserProfile is object)
                            arguments.Add($"--profile-directory=\"{browserProfile.Identifier}\"");

                        break;

                    case WebBrowserId.Firefox:

                        if (browserProfile is object) {

                            arguments.Add($"-profile \"{browserProfile.DirectoryPath}\"");

                        }
                        else if (!string.IsNullOrWhiteSpace(options.UserDataDirectoryPath)) {

                            // Firefox takes a profile path directly instead of a user data path (i.e. "Profiles" path).
                            // If the given directory doesn't have any profile information, we'll generate it.

                            FirefoxUtilities.CreateFirefoxUserDataDirectory(options.UserDataDirectoryPath);

                            IWebBrowserProfilesReader profilesReader = new FirefoxProfilesReader(options.UserDataDirectoryPath);

                            IWebBrowserProfile profile = profilesReader.GetDefaultProfile() ??
                                profilesReader.GetProfiles().FirstOrDefault();

                            if (profile is object)
                                arguments.Add($"-profile \"{profile.DirectoryPath}\"");

                        }

                        break;

                }

            }

            using (Process process = new Process()) {

                process.StartInfo = new ProcessStartInfo() {
                    FileName = browserExecutablePath,
                    Arguments = string.Join(" ", arguments),
                };

                bool processedStarted = process.Start();

                try {

                    // We can't rely on the return value of Start() alone, because it only returns true if we started a new process.
                    // It's possible that we're opening a new tab in an existing process as well, so it's important to also check the Responding property. 

                    return processedStarted ||
                        process.Responding;

                }
                catch (InvalidOperationException) {

                    // Accessing the Responding property will sometimes, nondeterministically, throw an exception (I'm not totally sure why).
                    // When this happens, we'll just return false to indicate that an error has occurred.

                    return false;

                }

            }

        }

        // Private members

        private static string PercentEncodeChars(string stringToEscape, char[] chars, bool encodeNonAsciiChars) {

            if (string.IsNullOrEmpty(stringToEscape))
                return stringToEscape;

            List<string> charPatterns = new List<string>(2);

            if (chars is object)
                charPatterns.Add($"[{string.Join(string.Empty, chars.Select(c => Regex.Escape(c.ToString())))}]");

            if (encodeNonAsciiChars)
                charPatterns.Add(@"[^\x00-\x7F]");

            string pattern = string.Join("|", charPatterns);

            return Regex.Replace(stringToEscape, pattern, m => {

                byte[] bytes = Encoding.UTF8.GetBytes(m.Value);

                return string.Join(string.Empty, bytes.Select(b => $"%{b:X2}"));

            });

        }

    }

}