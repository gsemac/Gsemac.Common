using Gsemac.Net.Properties;
using Gsemac.Polyfills.System.Threading;
using System;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Http.Extensions {

    public static class HttpListenerExtensions {

        // Public members

        public static HttpListenerContext GetContext(this HttpListener httpListener, TimeSpan timeout) {

            return GetContext(httpListener, timeout, CancellationToken.None);

        }
        public static HttpListenerContext GetContext(this HttpListener httpListener, CancellationToken cancellationToken) {

            return GetContext(httpListener, TimeoutEx.InfiniteTimeSpan, cancellationToken);

        }
        public static HttpListenerContext GetContext(this HttpListener httpListener, TimeSpan timeout, CancellationToken cancellationToken) {

            if (httpListener is null)
                throw new ArgumentNullException(nameof(httpListener));

            HttpListenerContext context = null;

            using (ManualResetEvent resetEvent = new ManualResetEvent(false)) {

                IAsyncResult asyncResult = httpListener.BeginGetContext(result => {

                    // A request has been received, so retrieve the context.
                    // Avoid the call to EndGetContext if cancellation is requested, because the listener might already be closed.

                    if (result is object && !cancellationToken.IsCancellationRequested) {

                        context = httpListener.EndGetContext(result);

                        resetEvent.Set();

                    }

                }, null);

                // You'd think we'd be able to use the WaitHandle on the AsyncResult instead, but we can't.
                // https://stackoverflow.com/a/4099757/5383169
                // We have to create a new wait handle manually.

                int operationResult = WaitHandle.WaitAny(new[] {
                    resetEvent,
                    cancellationToken.WaitHandle,
                }, timeout);

                bool operationTimedOut = operationResult == WaitHandle.WaitTimeout;
                bool operationCancelled = operationResult != 0;

                if (operationTimedOut || operationCancelled) {

                    // Cancel any pending requests by stopping and restarting the listener.

                    if (httpListener.IsListening) {

                        httpListener.Stop();
                        httpListener.Start();

                    }

                    if (operationTimedOut)
                        throw new TimeoutException(ExceptionMessages.HttpListenerTimedOut);

                    if (operationCancelled)
                        throw new OperationCanceledException(cancellationToken);

                }

                return context;

            }

        }

    }

}