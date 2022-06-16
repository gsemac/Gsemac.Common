﻿using Gsemac.Polyfills.System.Threading.Tasks;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Gsemac.Net.Http.Extensions {

    public static class HttpListenerExtensions {

        // Public members

        public static HttpListenerContext GetContext(this HttpListener httpListener, TimeSpan timeout) {

            HttpListenerContext context = null;

            IAsyncResult asyncResult = httpListener.BeginGetContext(_ => { }, null);

            Task.WaitAny(Task.Factory.StartNew(() => context = httpListener.EndGetContext(asyncResult)), TaskEx.Delay(timeout));

            return context;

        }

    }

}