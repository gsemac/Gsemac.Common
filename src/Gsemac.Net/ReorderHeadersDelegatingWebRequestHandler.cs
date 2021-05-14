﻿using Gsemac.Collections.Extensions;
using Gsemac.Net.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Gsemac.Net {

    // Despite the RFC stating that header ordering doesn't matter, some CDNs will block requests if the headers are not in a certain order (e.g. Cloudflare).
    // See https://sansec.io/research/http-header-order-is-important

    public class ReorderHeadersDelegatingWebRequestHandler :
        DelegatingWebRequestHandler {

        // Public members

        public ReorderHeadersDelegatingWebRequestHandler() :
            base() {
        }
        public ReorderHeadersDelegatingWebRequestHandler(WebRequestHandler innerHandler) :
            base(innerHandler) {
        }

        // Protected members

        protected internal override WebResponse Send(WebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            TrySetWebHeaderCollection(request);

            return base.Send(request, cancellationToken);

        }

        // Private members

        private class ReorderedWebHeaderCollection :
            WebHeaderCollection {

            // Public members

            public ReorderedWebHeaderCollection(HttpWebRequest webRequest) {

                // Store the web request so we can use its properties to generate headers even if they're changed after this object was instantiated.

                this.webRequest = webRequest;

                // The original headers will be lost when we set the new WebHeaderCollection, so we need to copy them immediately.

                webRequest.Headers.CopyTo(this);

            }

            public override string ToString() {

                IList<IHttpHeader> headers = new List<IHttpHeader>(this.GetHeaders());

                // Add required headers that are normally added automatically (they won't be added by HttpWebRequest).

                if (!headers.Any(h => h.Name.Equals("Host", StringComparison.OrdinalIgnoreCase)))
                    headers.Add(new HttpHeader("Host", Url.GetHostname(webRequest.RequestUri.AbsoluteUri)));

                if (webRequest.KeepAlive && !headers.Any(h => h.Name.Equals("Connection", StringComparison.OrdinalIgnoreCase)))
                    headers.Add(new HttpHeader("Connection", "keep-alive"));

                StringBuilder sb = new StringBuilder();

                foreach (IHttpHeader header in headers.OrderBy(h => GetHeaderOrder(h))) {

                    sb.Append(header.Name);
                    sb.Append(": ");
                    sb.Append(header.Value);
                    sb.Append("\r\n");

                }

                sb.Append("\r\n");

                return sb.ToString();

            }

            // Private members

            private readonly HttpWebRequest webRequest;

            private int GetHeaderOrder(IHttpHeader header) {

                string[] headerOrder = new[] {
                    "host",
                    "connection",
                    "accept",
                    "user-agent",
                };

                int order = headerOrder.IndexOf(header.Name.ToLowerInvariant());

                return order < 0 ?
                    headerOrder.Length :
                    order;

            }

        }

        private bool TrySetWebHeaderCollection(WebRequest webRequest) {

            if (webRequest is HttpWebRequestWrapper webRequestWrapper)
                webRequest = webRequestWrapper.GetUnderlyingWebRequest();

            if (webRequest is HttpWebRequest httpWebRequest) {

                FieldInfo fieldInfo = typeof(HttpWebRequest).GetField("_HttpRequestHeaders", BindingFlags.Instance | BindingFlags.NonPublic);

                if (fieldInfo is null)
                    return false;

                fieldInfo.SetValue(httpWebRequest, new ReorderedWebHeaderCollection(httpWebRequest));

                return true;

            }
            else {

                return false;

            }

        }

    }

}