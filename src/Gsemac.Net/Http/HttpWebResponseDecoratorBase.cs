using System;
using System.IO;
using System.Net;

namespace Gsemac.Net.Http {

    public abstract class HttpWebResponseDecoratorBase :
        WebResponse,
        IHttpWebResponse {

        // Public members

        // Properties overidden from WebRequest

        public override long ContentLength {
            get => innerHttpWebResponse.ContentLength;
            set => innerHttpWebResponse.ContentLength = value;
        }
        public override WebHeaderCollection Headers => innerHttpWebResponse.Headers;
        public override string ContentType {
            get => innerHttpWebResponse.ContentType;
            set => innerHttpWebResponse.ContentType = value;
        }
        public override bool IsFromCache => innerHttpWebResponse.IsFromCache;
        public override bool IsMutuallyAuthenticated => innerHttpWebResponse.IsMutuallyAuthenticated;
        public override Uri ResponseUri => innerHttpWebResponse.ResponseUri;

        // Properties inherited from IHttpWebResponse

        public string CharacterSet => innerHttpWebResponse.CharacterSet;
        public virtual CookieCollection Cookies {
            get => innerHttpWebResponse.Cookies;
            set => innerHttpWebResponse.Cookies = value;
        }
        public string ContentEncoding => innerHttpWebResponse.ContentEncoding;
        public Version ProtocolVersion => innerHttpWebResponse.ProtocolVersion;
        public DateTime LastModified => innerHttpWebResponse.LastModified;
        public string Method => innerHttpWebResponse.Method;
        public string Server => innerHttpWebResponse.Server;
        public HttpStatusCode StatusCode => innerHttpWebResponse.StatusCode;
        public string StatusDescription => innerHttpWebResponse.StatusDescription;

        public override Stream GetResponseStream() {

            return innerHttpWebResponse.GetResponseStream();

        }
        public override void Close() {

            innerHttpWebResponse.Close();

        }

        public string GetResponseHeader(string headerName) {

            return innerHttpWebResponse.GetResponseHeader(headerName);

        }

        // Internal members

        internal WebResponse InnerWebResponse => (WebResponse)innerHttpWebResponse;

        // Protected members

        protected HttpWebResponseDecoratorBase(IHttpWebResponse innerHttpWebResponse) {

            if (innerHttpWebResponse is null)
                throw new ArgumentNullException(nameof(innerHttpWebResponse));

            this.innerHttpWebResponse = innerHttpWebResponse;

        }

        // Private members

        private readonly IHttpWebResponse innerHttpWebResponse;

    }

}