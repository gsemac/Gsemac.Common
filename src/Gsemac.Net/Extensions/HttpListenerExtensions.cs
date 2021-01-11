using System;
using System.Net;
using System.Threading.Tasks;

namespace Gsemac.Net.Extensions {

    public static class HttpListenerExtensions {

        public static HttpListenerContext GetContext(this HttpListener httpListener, TimeSpan timeout) {

            HttpListenerContext context = null;

            IAsyncResult asyncResult = httpListener.BeginGetContext(_ => { }, null);

            Task.WaitAny(Task.Factory.StartNew(() => context = httpListener.EndGetContext(asyncResult)), Polyfills.System.Threading.Tasks.Task.Delay(timeout));

            return context;

        }

    }

}