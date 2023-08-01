using Gsemac.Net.Properties;
using Gsemac.Polyfills.System.Threading.Tasks;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net.Http.Extensions {

    public static class HttpListenerExtensions {

        // Public members

        public static HttpListenerContext GetContext(this HttpListener httpListener, TimeSpan timeout) {

            return GetContext(httpListener, timeout, CancellationToken.None);

        }
        public static HttpListenerContext GetContext(this HttpListener httpListener, TimeSpan timeout, CancellationToken cancellationToken) {

            if (httpListener is null)
                throw new ArgumentNullException(nameof(httpListener));

            HttpListenerContext context = null;
            bool operationTimedOut = false;

            IAsyncResult asyncResult = httpListener.BeginGetContext(result => {

                // A request has been received, so retrieve the context.

                if (result is object)
                    context = httpListener.EndGetContext(result);

            }, null);

            Task[] tasks = new[] {
                TaskEx.Run(() => operationTimedOut =! asyncResult.AsyncWaitHandle.WaitOne(timeout), cancellationToken),
                TaskEx.Delay(timeout, cancellationToken),
            };

            try {

                if (Task.WaitAny(tasks, cancellationToken) != 0 || operationTimedOut)
                    throw new TimeoutException(ExceptionMessages.HttpListenerTimedOut);

            }
            catch (Exception) {

                // Cancel any pending requests by stopping and restarting the listener.

                if (httpListener.IsListening) {

                    httpListener.Stop();
                    httpListener.Start();

                }

                throw;

            }

            return context;

        }

    }

}