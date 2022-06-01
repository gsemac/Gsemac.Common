using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Principal;

namespace Gsemac.Net {

    public class WebRequestAdapter :
        WebRequest,
        IWebRequest {

        // Public members

        public override string Method {
            get => webRequest.Method;
            set => webRequest.Method = value;
        }
        public new AuthenticationLevel AuthenticationLevel {
            get => webRequest.AuthenticationLevel;
            set => webRequest.AuthenticationLevel = value;
        }
        public override int Timeout {
            get => webRequest.Timeout;
            set => webRequest.Timeout = value;
        }
        public override bool PreAuthenticate {
            get => webRequest.PreAuthenticate;
            set => webRequest.PreAuthenticate = value;
        }
        public override IWebProxy Proxy {
            get => webRequest.Proxy;
            set => webRequest.Proxy = value;
        }
        public override bool UseDefaultCredentials {
            get => webRequest.UseDefaultCredentials;
            set => webRequest.UseDefaultCredentials = value;
        }
        public override ICredentials Credentials {
            get => webRequest.Credentials;
            set => webRequest.Credentials = value;
        }
        public override string ContentType {
            get => webRequest.ContentType;
            set => webRequest.ContentType = value;
        }
        public override long ContentLength {
            get => webRequest.ContentLength;
            set => webRequest.ContentLength = value;
        }
        public override WebHeaderCollection Headers {
            get => webRequest.Headers;
            set => webRequest.Headers = value;
        }
        public override string ConnectionGroupName {
            get => webRequest.ConnectionGroupName;
            set => webRequest.ConnectionGroupName = value;
        }
        public new TokenImpersonationLevel ImpersonationLevel {
            get => webRequest.ImpersonationLevel;
            set => webRequest.ImpersonationLevel = value;
        }
        public override RequestCachePolicy CachePolicy {
            get => webRequest.CachePolicy;
            set => webRequest.CachePolicy = value;
        }
        public override Uri RequestUri => webRequest.RequestUri;

        public WebRequestAdapter(WebRequest webRequest) {

            if (webRequest is null)
                throw new ArgumentNullException(nameof(webRequest));

            // Since we can't override these properties, we'll just copy them in the constructor.

            AuthenticationLevel = webRequest.AuthenticationLevel;
            ImpersonationLevel = webRequest.ImpersonationLevel;

            this.webRequest = webRequest;

        }

        public override void Abort() => webRequest.Abort();
        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state) => webRequest.BeginGetRequestStream(callback, state);
        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state) => webRequest.BeginGetResponse(callback, state);
        public override Stream EndGetRequestStream(IAsyncResult asyncResult) => webRequest.EndGetRequestStream(asyncResult);
        public override WebResponse EndGetResponse(IAsyncResult asyncResult) => webRequest.EndGetResponse(asyncResult);
        public override Stream GetRequestStream() => webRequest.GetRequestStream();
        public override WebResponse GetResponse() => webRequest.GetResponse();

        // Private members

        private readonly WebRequest webRequest;

    }

}