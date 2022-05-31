using Gsemac.IO;
using Gsemac.Net.Http;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Extensions {

    public static class WebClientExtensions {

        // Public members

        public static string DownloadFile(this WebClient client, Uri address) {

            return DownloadFile(new WebClientAdapter(client), address);

        }
        public static string DownloadFile(this IWebClient client, Uri address) {

            string filename = GetFilenameFromUri(address);

            client.DownloadFile(address, filename);

            return filename;

        }
        public static string DownloadFile(this WebClient client, string address) {

            return DownloadFile(new WebClientAdapter(client), address);

        }
        public static string DownloadFile(this IWebClient client, string address) {

            return DownloadFile(client, new Uri(address));

        }
        public static void DownloadFileAsync(this WebClient client, Uri address) {

            DownloadFileAsync(new WebClientAdapter(client), address);

        }
        public static void DownloadFileAsync(this IWebClient client, Uri address) {

            client.DownloadFileAsync(address, GetFilenameFromUri(address));

        }

        public static void DownloadFileSync(this WebClient client, Uri address, string filename) {

            DownloadFileSync(new WebClientAdapter(client), address, filename);

        }
        public static void DownloadFileSync(this WebClient client, Uri address, string filename, CancellationToken cancellationToken) {

            DownloadFileSync(new WebClientAdapter(client), address, filename, cancellationToken);

        }
        public static void DownloadFileSync(this IWebClient client, Uri address, string filename) {

            DownloadFileSync(client, address, filename, CancellationToken.None);

        }
        public static void DownloadFileSync(this IWebClient client, Uri address, string filename, CancellationToken cancellationToken) {

            if (cancellationToken.IsCancellationRequested)
                throw new WebException(Properties.ExceptionMessages.RequestCanceled, WebExceptionStatus.RequestCanceled);

            DownloadFileSyncInternal(client, address, filename, cancellationToken);

        }
        public static void DownloadFileSync(this WebClient client, string address, string filename) {

            DownloadFileSync(new WebClientAdapter(client), address, filename);

        }
        public static void DownloadFileSync(this WebClient client, string address, string filename, CancellationToken cancellationToken) {

            DownloadFileSync(new WebClientAdapter(client), address, filename, cancellationToken);

        }
        public static void DownloadFileSync(this IWebClient client, string address, string filename) {

            DownloadFileSync(client, new Uri(address), filename);

        }
        public static void DownloadFileSync(this IWebClient client, string address, string filename, CancellationToken cancellationToken) {

            DownloadFileSync(client, new Uri(address), filename, cancellationToken);

        }

        public static WebClient WithOptions(this WebClient client, IHttpWebRequestOptions options) {

            WithOptions(new WebClientAdapter(client), options);

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

        // Private members

        private static void DownloadFileSyncInternal(IWebClient client, Uri address, string filename, CancellationToken cancellationToken) {

            if (cancellationToken.IsCancellationRequested)
                throw new WebException(Properties.ExceptionMessages.RequestCanceled, WebExceptionStatus.RequestCanceled);

            // Events are only fired when downloading asynchronously with DownloadFileAsync.
            // This method allows for synchronous downloads while still firing events. 
            // Based on the solution given here: https://stackoverflow.com/a/25834736 (user195275)

            object mutex = new object();
            bool downloadCancelled = false;

            void handleDownloadComplete(object sender, AsyncCompletedEventArgs e) {

                downloadCancelled = e.Cancelled;

                lock (e.UserState)
                    Monitor.Pulse(e.UserState);

            }

            client.DownloadFileCompleted += handleDownloadComplete;

            try {

                lock (mutex) {

                    using (cancellationToken.Register(() => {

                        // The download was canceled.

                        // Note that CancelAsync doesn't actually work if the download has already started: https://github.com/dotnet/runtime/issues/31479 
                        // It's my understanding that this is not a problem in newer versions of .NET Framework (4.7.2+), although it has resurfaced in .NET Core (2.0+).
                        // For this reason, we need to manually unblock the waiting thread.

                        client.CancelAsync();

                        // Unblock the waiting thread, and flag the download as canceled.

                        lock (mutex)
                            Monitor.Pulse(mutex);

                        downloadCancelled = true;

                    })) {

                        // Initiate the asynchronous download process and wait for its completion.

                        client.DownloadFileAsync(address, filename, mutex);

                        Monitor.Wait(mutex);

                    }

                }

            }
            catch (ThreadInterruptedException ex) {

                // If the waiting thread is interrupted, treat it as if the download was canceled.
                // We will only end up here if this method was called in a new thread that was interrupted (via Thread.Interrupt) prior to download completion.

                throw new WebException(Properties.ExceptionMessages.RequestCanceled, ex, WebExceptionStatus.RequestCanceled, response: null);

            }
            finally {

                client.DownloadFileCompleted -= handleDownloadComplete;

            }

            if (downloadCancelled) {

                // The DownloadFile method deletes partially-downloaded files if the download is not successful.
                // https://referencesource.microsoft.com/#system/net/System/Net/webclient.cs,416

                if (downloadCancelled && File.Exists(filename))
                    FileUtilities.TryDelete(filename);

                throw new WebException(Properties.ExceptionMessages.RequestCanceled, WebExceptionStatus.RequestCanceled);

            }

        }

        private static string GetFilenameFromUri(Uri address) {

            return PathUtilities.SanitizePath(PathUtilities.GetFilename(address.AbsoluteUri), new SanitizePathOptions() {
                StripInvalidFilenameChars = true,
            });

        }

    }

}