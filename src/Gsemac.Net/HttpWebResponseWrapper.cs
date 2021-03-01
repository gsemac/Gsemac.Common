using System;
using System.IO;
using System.Net;

namespace Gsemac.Net {

    public class HttpWebResponseWrapper :
        WebResponse,
        IHttpWebResponse {

        // Public members

        public HttpWebResponseWrapper(WebResponse webResponse) {

            if (webResponse is null)
                throw new ArgumentNullException(nameof(webResponse));

            if (webResponse is HttpWebResponse httpWebResponse)
                this.httpWebResponse = httpWebResponse;
            else if (webResponse is HttpWebResponseWrapper httpWebResponseWrapper)
                this.httpWebResponse = httpWebResponseWrapper.httpWebResponse;
            else
                throw new ArgumentException("WebResponse was not an instance of HttpWebResponse.", nameof(webResponse));

        }
        public HttpWebResponseWrapper(HttpWebResponse httpWebResponse) {

            if (httpWebResponse is null)
                throw new ArgumentNullException(nameof(httpWebResponse));

            this.httpWebResponse = httpWebResponse;

        }

        // Private members

        private readonly HttpWebResponse httpWebResponse;

        // Inherited from WebResponse

        public override long ContentLength {
            get => httpWebResponse.ContentLength;
            set => httpWebResponse.ContentLength = value;
        }
        public override string ContentType {
            get => httpWebResponse.ContentType;
            set => httpWebResponse.ContentType = value;
        }
        public override WebHeaderCollection Headers => httpWebResponse.Headers;
        public override bool IsFromCache => httpWebResponse.IsFromCache;
        public override bool IsMutuallyAuthenticated => httpWebResponse.IsMutuallyAuthenticated;
        public override Uri ResponseUri => httpWebResponse.ResponseUri;

        public override void Close() => httpWebResponse.Close();
        public override Stream GetResponseStream() => httpWebResponse.GetResponseStream();

        // Inherited from IHttpWebResponse

        public string ContentEncoding => httpWebResponse.ContentEncoding;
        public Version ProtocolVersion => httpWebResponse.ProtocolVersion;
        public string StatusDescription => httpWebResponse.StatusDescription;
        public HttpStatusCode StatusCode => httpWebResponse.StatusCode;
        public DateTime LastModified => httpWebResponse.LastModified;
        public string Server => httpWebResponse.Server;
        public string CharacterSet => httpWebResponse.CharacterSet;
        public string Method => httpWebResponse.Method;
        public CookieCollection Cookies {
            get => httpWebResponse.Cookies;
            set => httpWebResponse.Cookies = value;
        }

        public string GetResponseHeader(string headerName) => httpWebResponse.GetResponseHeader(headerName);

    }

}