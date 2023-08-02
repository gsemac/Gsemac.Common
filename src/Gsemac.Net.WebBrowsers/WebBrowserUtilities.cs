using Gsemac.IO;
using Gsemac.Net.Extensions;
using Gsemac.Net.Http.Extensions;
using Gsemac.Net.Sockets;
using Gsemac.Net.WebBrowsers.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

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
            bool listenForRequests = true;

            using (HttpListener listener = new HttpListener()) {

                listener.Prefixes.Add(requestUri.AbsoluteUri);

                listener.Start();

                // Open the HTTP server in the user's web browser.

                Process.Start(webBrowserInfo.ExecutablePath, requestUri.AbsoluteUri);

                // Wait for an incoming request.

                while (listenForRequests) {

                    listenForRequests = false;

                    HttpListenerContext context = listener.GetContext(timeout);

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
            string browserExecutablePath = browserInfo?.ExecutablePath ?? "explorer.exe";
            string arguments = $"\"{url}\"";

            if (browserInfo is object && browserProfile is object) {

                switch (browserInfo.Id) {

                    case WebBrowserId.Chrome:
                    case WebBrowserId.Edge:

                        arguments = $"\"{url}\" --profile-directory=\"{browserProfile.Identifier}\"";

                        break;

                    case WebBrowserId.Firefox:

                        arguments = $"\"{url}\" -profile=\"{browserProfile.DirectoryPath}\"";

                        break;

                }

            }

            using (Process process = Process.Start(browserExecutablePath, arguments))
                return process.Responding;

        }

    }

}