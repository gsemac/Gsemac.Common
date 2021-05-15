using Gsemac.IO;
using System;
using System.ComponentModel;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Extensions {

    public static class WebClientExtensions {

        public static void DownloadFile(this WebClient client, Uri address) {

            DownloadFile(new WebClientWrapper(client), address);

        }
        public static void DownloadFile(this IWebClient client, Uri address) {

            client.DownloadFile(address, PathUtilities.SanitizePath(PathUtilities.GetFilename(address.AbsoluteUri), SanitizePathOptions.StripInvalidFilenameChars));

        }
        public static void DownloadFile(this WebClient client, string address) {

            DownloadFile(new WebClientWrapper(client), address);

        }
        public static void DownloadFile(this IWebClient client, string address) {

            DownloadFile(client, new Uri(address));

        }
        public static void DownloadFileAsync(this WebClient client, Uri address) {

            DownloadFileAsync(new WebClientWrapper(client), address);

        }
        public static void DownloadFileAsync(this IWebClient client, Uri address) {

            client.DownloadFileAsync(address, PathUtilities.GetFilename(address.AbsoluteUri));

        }

        public static void DownloadFileSync(this WebClient client, Uri address, string filename) {

            DownloadFileSync(new WebClientWrapper(client), address, filename);

        }
        public static void DownloadFileSync(this WebClient client, Uri address, string filename, CancellationToken cancellationToken) {

            DownloadFileSync(new WebClientWrapper(client), address, filename, cancellationToken);

        }
        public static void DownloadFileSync(this IWebClient client, Uri address, string filename) {

            DownloadFileSync(client, address, filename, CancellationToken.None);

        }
        public static void DownloadFileSync(this IWebClient client, Uri address, string filename, CancellationToken cancellationToken) {

            // Events are only fired when downloading asynchronously with DownloadFileAsync.
            // This method allows for synchronous downloads while still firing events. 
            // Based on the solution given here: https://stackoverflow.com/a/25834736 (user195275)

            object mutex = new object();

            void handleDownloadComplete(object sender, AsyncCompletedEventArgs e) {

                lock (e.UserState)
                    Monitor.Pulse(e.UserState);

            }

            client.DownloadFileCompleted += handleDownloadComplete;

            if (cancellationToken != CancellationToken.None)
                cancellationToken.Register(() => client.CancelAsync());

            lock (mutex) {

                client.DownloadFileAsync(address, filename, mutex);

                Monitor.Wait(mutex);

            }

            client.DownloadFileCompleted -= handleDownloadComplete;

        }
        public static void DownloadFileSync(this WebClient client, string address, string filename) {

            DownloadFileSync(new WebClientWrapper(client), address, filename);

        }
        public static void DownloadFileSync(this WebClient client, string address, string filename, CancellationToken cancellationToken) {

            DownloadFileSync(new WebClientWrapper(client), address, filename, cancellationToken);

        }
        public static void DownloadFileSync(this IWebClient client, string address, string filename) {

            DownloadFileSync(client, new Uri(address), filename);

        }
        public static void DownloadFileSync(this IWebClient client, string address, string filename, CancellationToken cancellationToken) {

            DownloadFileSync(client, new Uri(address), filename, cancellationToken);

        }

        public static WebClient WithOptions(this WebClient client, IHttpWebRequestOptions options) {

            WithOptions(new WebClientWrapper(client), options);

            return client;

        }
        public static IWebClient WithOptions(this IWebClient client, IHttpWebRequestOptions options) {

            client.Headers[HttpRequestHeader.Accept] = options.Accept;
            client.Headers[HttpRequestHeader.AcceptLanguage] = options.AcceptLanguage;
            //webClient.AutomaticDecompression = options.AutomaticDecompression;
            //webClient.CookieContainer = options.Cookies;
            client.Credentials = options.Credentials;
            client.Proxy = options.Proxy;
            client.Headers[HttpRequestHeader.UserAgent] = options.UserAgent;

            return client;

        }

    }

}