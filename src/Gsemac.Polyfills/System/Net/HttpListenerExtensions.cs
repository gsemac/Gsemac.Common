using System;
using System.Net;
using System.Threading.Tasks;

namespace Gsemac.Polyfills.System.Net {

    public static class HttpListenerExtensions {

        // Public members

        /// <summary>
        /// Waits for an incoming request as an asynchronous operation.
        /// </summary>
        public static Task<HttpListenerContext> GetContextAsync(this HttpListener httpListener) {

            if (httpListener is null)
                throw new ArgumentNullException(nameof(httpListener));

            return Task<HttpListenerContext>.Factory.FromAsync(httpListener.BeginGetContext, httpListener.EndGetContext, null);

        } // .NET Framework 4.5 and later

    }

}