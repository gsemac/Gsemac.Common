using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace Gsemac.Net {

    public abstract class HttpWebRequestBase :
        WebRequest,
        IHttpWebRequest {

        // Public members

        // Properties overidden from WebRequest

        public override long ContentLength {
            get {

                if (long.TryParse(Headers[HttpRequestHeader.ContentLength], out long result))
                    return result;

                return 0;

            }
            set => Headers[HttpRequestHeader.ContentLength] = value.ToString();
        }
        public override string ContentType {
            get => Headers[HttpRequestHeader.ContentType];
            set => Headers[HttpRequestHeader.ContentType] = value;
        }
        public override ICredentials Credentials { get; set; }
        public override WebHeaderCollection Headers { get; set; } = new WebHeaderCollection();
        public override string Method { get; set; } = "GET";
        public override IWebProxy Proxy { get; set; } = WebProxyUtilities.GetDefaultProxy();
        public override Uri RequestUri { get; }
        public override int Timeout { get; set; } = (int)TimeSpan.FromSeconds(100).TotalMilliseconds; // 100 seconds is the default for HttpWebRequest

        public new AuthenticationLevel AuthenticationLevel {
            get => base.AuthenticationLevel;
            set => base.AuthenticationLevel = value;
        }
        public new TokenImpersonationLevel ImpersonationLevel {
            get => base.ImpersonationLevel;
            set => base.ImpersonationLevel = value;
        }

        public override Stream GetRequestStream() => GetRequestStream(validateMethod: true);
        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state) {

            return getResponseDelegate.BeginInvoke(callback, state);

        }
        public override WebResponse EndGetResponse(IAsyncResult asyncResult) {

            return getResponseDelegate.EndInvoke(asyncResult);

        }

        // Properties inherited from IHttpWebRequest

        public string Accept {
            get => Headers[HttpRequestHeader.Accept];
            set => Headers[HttpRequestHeader.Accept] = value;
        }
        public Uri Address => RequestUri;
        public bool AllowAutoRedirect { get; set; } = true;
        public bool AllowWriteStreamBuffering { get; set; } = true;
        public DecompressionMethods AutomaticDecompression { get; set; } = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        public string Connection {
            get => Headers[HttpRequestHeader.Connection];
            set => Headers[HttpRequestHeader.Connection] = value;
        }
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();
        public DateTime Date {
            get => DateTime.Parse(Headers[HttpRequestHeader.Date]);
            set => Headers[HttpRequestHeader.Date] = string.Format("{0:ddd,' 'dd' 'MMM' 'yyyy' 'HH':'mm':'ss' 'K}", value);
        }
        public string Expect {
            get => Headers[HttpRequestHeader.Expect];
            set => Headers[HttpRequestHeader.Expect] = value;
        }
        public bool HaveResponse { get; protected set; } = false;
        public string Host {
            get => Headers[HttpRequestHeader.Host];
            set => Headers[HttpRequestHeader.Host] = value;
        }
        public DateTime IfModifiedSince {
            get => DateTime.Parse(Headers[HttpRequestHeader.IfModifiedSince]);
            set => Headers[HttpRequestHeader.IfModifiedSince] = string.Format("{0:ddd,' 'dd' 'MMM' 'yyyy' 'HH':'mm':'ss' 'K}", value);
        }
        public bool KeepAlive { get; set; } = true;
        public int MaximumAutomaticRedirections { get; set; } = 50;
        public int MaximumResponseHeadersLength { get; set; } = HttpWebRequest.DefaultMaximumResponseHeadersLength;
        public bool Pipelined { get; set; } = true;
        public Version ProtocolVersion { get; set; } = new Version(2, 0);
        public int ReadWriteTimeout { get; set; } = 300000;
        public string Referer {
            get => Headers[HttpRequestHeader.Referer];
            set => Headers[HttpRequestHeader.Referer] = value;
        }
        public bool SendChunked { get; set; } = false;
        public string TransferEncoding {
            get => Headers[HttpRequestHeader.TransferEncoding];
            set => Headers[HttpRequestHeader.TransferEncoding] = value;
        }
        public bool UnsafeAuthenticatedConnectionSharing { get; set; } = false;
        public string UserAgent {
            get => Headers[HttpRequestHeader.UserAgent];
            set => Headers[HttpRequestHeader.UserAgent] = value;
        }

        public X509CertificateCollection ClientCertificates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public HttpContinueDelegate ContinueDelegate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ServicePoint ServicePoint => throw new NotImplementedException();
        public string MediaType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddRange(int from, int to) => AddRange((long)from, to);
        public void AddRange(long from, long to) => AddRange("bytes", from, to);
        public void AddRange(int range) => AddRange((long)range);
        public void AddRange(long range) => AddRange("bytes", range);
        public void AddRange(string rangeSpecifier, long from, long to) {

            Headers[HttpRequestHeader.Range] = new RangeHeaderBuilder(Headers[HttpRequestHeader.Range])
                .AddRange(rangeSpecifier, from, to)
                .ToString();

        }
        public void AddRange(string rangeSpecifier, int range) => AddRange(rangeSpecifier, (long)range);
        public void AddRange(string rangeSpecifier, long range) {

            Headers[HttpRequestHeader.Range] = new RangeHeaderBuilder(Headers[HttpRequestHeader.Range])
                .AddRange(rangeSpecifier, range)
                .ToString();

        }
        public void AddRange(string rangeSpecifier, int from, int to) => AddRange(rangeSpecifier, (long)from, to);

        // Protected members

        private delegate WebResponse GetResponseDelegate();

        protected HttpWebRequestBase(Uri requestUri) {

            this.RequestUri = requestUri;

            getResponseDelegate = new GetResponseDelegate(GetResponse);

        }

        protected Stream GetRequestStream(bool validateMethod) {

            if (validateMethod) {

                if (string.IsNullOrEmpty(Method))
                    Method = "POST";

                if (!Method.Equals("POST", StringComparison.OrdinalIgnoreCase) && !Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
                    throw new ProtocolViolationException("Method must be POST or PUT.");

            }

            if (requestStream is null)
                requestStream = new MemoryStream();

            return requestStream;

        }

        // Private members

        private MemoryStream requestStream;
        private readonly GetResponseDelegate getResponseDelegate;

    }

}