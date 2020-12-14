using Gsemac.Net.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Gsemac.Net.WebBrowsers {

    public static class WebBrowserUtilities {

        // Public members

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
                throw new FileNotFoundException();

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

    }

}