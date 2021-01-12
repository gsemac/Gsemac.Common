using Gsemac.IO;
using System;
using System.ComponentModel;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Extensions {

    public static class WebClientExtensions {

        public static void DownloadFile(this WebClient client, Uri address) {

            client.DownloadFile(address, PathUtilities.GetFileName(address.AbsoluteUri));

        }
        public static void DownloadFileAsync(this WebClient client, Uri address) {

            client.DownloadFileAsync(address, PathUtilities.GetFileName(address.AbsoluteUri));

        }

        public static void DownloadFileSync(this WebClient client, Uri address, string fileName) {

            // Events are only fired when downloading asynchronously with DownloadFileAsync.
            // This method allows for synchronous downloads while still firing events. 
            // Based on the solution given here: https://stackoverflow.com/a/25834736 (user195275)

            object mutex = new object();

            void handleDownloadComplete(object sender, AsyncCompletedEventArgs e) {

                lock (e.UserState)
                    Monitor.Pulse(e.UserState);

            }

            client.DownloadFileCompleted += handleDownloadComplete;

            lock (mutex) {

                client.DownloadFileAsync(address, fileName, mutex);

                Monitor.Wait(mutex);

            }

            client.DownloadFileCompleted -= handleDownloadComplete;

        }

        public static WebClient WithOptions(this WebClient client, IHttpWebRequestOptions options) {

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