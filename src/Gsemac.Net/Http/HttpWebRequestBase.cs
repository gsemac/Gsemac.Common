using Gsemac.Net.Http.Headers;
using Gsemac.Net.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Gsemac.Net.Http {

    public abstract class HttpWebRequestBase :
        WebRequest,
        IHttpWebRequest {

        // Public members

        // Properties overidden from WebRequest

        public override long ContentLength {
            get => GetContentLength();
            set => SetContentLength(value);
        }
        public override string ContentType {
            get => Headers[HttpRequestHeader.ContentType];
            set => SetOrRemoveHeader(HttpRequestHeader.ContentType, value);
        }
        public override ICredentials Credentials { get; set; }
        public override WebHeaderCollection Headers { get; set; } = new WebHeaderCollection();
        public override string Method { get; set; } = DefaultHttpHeaders.Method;
        public override IWebProxy Proxy { get; set; } = WebRequestUtilities.GetDefaultWebProxy();
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
            set => SetOrRemoveHeader(HttpRequestHeader.Accept, value);
        }
        public Uri Address => RequestUri;
        public bool AllowAutoRedirect { get; set; } = true;
        public bool AllowWriteStreamBuffering { get; set; } = true;
        public DecompressionMethods AutomaticDecompression { get; set; } = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        public string Connection {
            get => Headers[HttpRequestHeader.Connection];
            set => SetOrRemoveHeader(HttpRequestHeader.Connection, value);
        }
        public CookieContainer CookieContainer { get; set; } // CookieContainer is null by default for HttpWebRequest (Note that the "cookie" header is only sent when this property is null)
        public DateTime Date {
            get => HttpUtilities.ParseDate(Headers[HttpRequestHeader.Date]).DateTime;
            set => SetOrRemoveHeader(HttpRequestHeader.Date, value);
        }
        public string Expect {
            get => Headers[HttpRequestHeader.Expect];
            set => SetOrRemoveHeader(HttpRequestHeader.Expect, value);
        }
        public bool HaveResponse { get; protected set; } = false;
        public string Host {
            get => Headers[HttpRequestHeader.Host];
            set => SetOrRemoveHeader(HttpRequestHeader.Host, value);
        }
        public DateTime IfModifiedSince {
            get => HttpUtilities.ParseDate(Headers[HttpRequestHeader.IfModifiedSince]).DateTime;
            set => SetOrRemoveHeader(HttpRequestHeader.IfModifiedSince, value);
        }
        public bool KeepAlive { get; set; } = true;
        public int MaximumAutomaticRedirections { get; set; } = 50;
        public int MaximumResponseHeadersLength { get; set; } = HttpWebRequest.DefaultMaximumResponseHeadersLength;
        public bool Pipelined { get; set; } = true;
        public System.Version ProtocolVersion { get; set; } = new System.Version(1, 1);
        public int ReadWriteTimeout { get; set; } = 300000;
        public string Referer {
            get => Headers[HttpRequestHeader.Referer];
            set => SetOrRemoveHeader(HttpRequestHeader.Referer, value);
        }
        public bool SendChunked { get; set; } = false;
        public string TransferEncoding {
            get => Headers[HttpRequestHeader.TransferEncoding];
            set => SetOrRemoveHeader(HttpRequestHeader.TransferEncoding, value);
        }
        public bool UnsafeAuthenticatedConnectionSharing { get; set; } = false;
        public string UserAgent {
            get => Headers[HttpRequestHeader.UserAgent];
            set => SetOrRemoveHeader(HttpRequestHeader.UserAgent, value);
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

            RequestUri = requestUri;

            getResponseDelegate = new GetResponseDelegate(GetResponse);

        }

        protected Stream GetRequestStream(bool validateMethod) {

            if (validateMethod) {

                if (string.IsNullOrEmpty(Method))
                    Method = "POST";

                if (!Method.Equals("POST", StringComparison.OrdinalIgnoreCase) && !Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
                    throw new ProtocolViolationException(ExceptionMessages.MethodMustBePostOrPut);

            }

            if (requestStream is null)
                requestStream = new MemoryStream();

            return requestStream;

        }

        // Private members

        private MemoryStream requestStream;
        private readonly GetResponseDelegate getResponseDelegate;

        private long GetContentLength() {

            if (long.TryParse(Headers[HttpRequestHeader.ContentLength], out long result))
                return result;

            // The default HttpWebRequest implementation returns -1 when the content-length header has not been set.

            return -1;

        }
        private void SetContentLength(long value) {

            if (value < 0)
                throw new ArgumentException(ExceptionMessages.ContentLengthMustBeGreaterThanOrEqualToZero, nameof(value));

            Headers[HttpRequestHeader.ContentLength] = value.ToString(CultureInfo.InvariantCulture);

        }
        private void SetOrRemoveHeader(HttpRequestHeader header, string value) {

            // The default HttpWebRequest implementation removes headers that have been set to empty/whitespace strings.

            if (string.IsNullOrWhiteSpace(value)) {

                Headers.Remove(header);

            }
            else {

                Headers[header] = value;

            }

        }
        private void SetOrRemoveHeader(HttpRequestHeader header, DateTime date) {

            if (date == default) {

                Headers.Remove(header);

            }
            else {

                Headers[header] = date.ToUniversalTime().ToString("r");

            }

        }

    }

}