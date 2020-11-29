using System;
using System.IO;
using System.Net;

namespace Gsemac.Net {

    public abstract class HttpWebResponseBase :
        WebResponse,
        IHttpWebResponse {

        // Public members

        // Properties overidden from WebRequest

        public override long ContentLength {
            get {

                if (long.TryParse(Headers[HttpResponseHeader.ContentLength], out long result))
                    return result;

                // It's important to return -1 instead of 0, which lets classes like WebClient know there is still a body, just no content-length header.

                return -1;

            }
            set => Headers[HttpResponseHeader.ContentLength] = value.ToString();
        }
        public override WebHeaderCollection Headers => headers;
        public override string ContentType {
            get => Headers[HttpResponseHeader.ContentType];
            set => Headers[HttpResponseHeader.ContentType] = value;
        }
        public override bool IsFromCache { get; } = false;
        public override bool IsMutuallyAuthenticated { get; } = false;
        public override Uri ResponseUri => responseUri;

        // Properties inherited from IHttpWebResponse

        public string CharacterSet { get; protected set; }
        public virtual CookieCollection Cookies {
            get => cookies.GetCookies(ResponseUri);
            set => cookies.Add(value);
        }
        public string ContentEncoding {
            get => Headers[HttpResponseHeader.ContentEncoding];
            set => Headers[HttpResponseHeader.ContentEncoding] = value;
        }
        public Version ProtocolVersion { get; protected set; }
        public DateTime LastModified {
            get => DateTime.Parse(Headers[HttpResponseHeader.LastModified]);
            set => Headers[HttpResponseHeader.LastModified] = string.Format("{0:ddd,' 'dd' 'MMM' 'yyyy' 'HH':'mm':'ss' 'K}", value);
        }
        public string Method { get; protected set; }
        public string Server {
            get => Headers[HttpResponseHeader.Server];
            set => Headers[HttpResponseHeader.Server] = value;
        }
        public HttpStatusCode StatusCode { get; protected set; } = 0;
        public string StatusDescription { get; protected set; }

        public override Stream GetResponseStream() {

            return responseStream;

        }
        public override void Close() {

            // IMPORTANT NOTE: 
            // This method gets called by Dispose(), but WebClient doesn't call Dispose(). Instead, it calls Close() on the response stream directly (bad design IMO).  
            // As a result, it's important that this method doesn't contain any important logic, and ONLY closes the response stream.

            if (!streamClosed)
                responseStream.Close();

            streamClosed = true;

        }

        public string GetResponseHeader(string headerName) {

            return Headers[headerName];

        }

        // Protected members

        protected HttpWebResponseBase(Uri responseUri, Stream responseStream) {

            this.responseUri = responseUri;
            this.responseStream = responseStream;

        }

        // Private members

        private readonly Uri responseUri;
        private readonly WebHeaderCollection headers = new WebHeaderCollection();
        private readonly CookieContainer cookies = new CookieContainer();
        private readonly Stream responseStream;
        private bool streamClosed = false;

    }

}