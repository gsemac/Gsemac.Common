using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;

namespace Gsemac.Net {

    public class HttpWebRequestWrapper :
        WebRequest,
        IHttpWebRequest {

        // Public members

        public HttpWebRequestWrapper(WebRequest webRequest) {

            if (webRequest is null)
                throw new ArgumentNullException(nameof(webRequest));

            if (webRequest is HttpWebRequest httpWebRequest)
                this.httpWebRequest = httpWebRequest;
            else
                throw new ArgumentException("WebRequest was not an instance of class HttpWebRequest.", nameof(webRequest));

        }
        public HttpWebRequestWrapper(HttpWebRequest httpWebRequest) {

            if (httpWebRequest is null)
                throw new ArgumentNullException(nameof(httpWebRequest));

            this.httpWebRequest = httpWebRequest;

            AuthenticationLevel = httpWebRequest.AuthenticationLevel;
            ImpersonationLevel = httpWebRequest.ImpersonationLevel;

        }

        // Private members

        private readonly HttpWebRequest httpWebRequest;

        // Inherited from WebRequest

        public override bool PreAuthenticate {
            get => httpWebRequest.PreAuthenticate;
            set => httpWebRequest.PreAuthenticate = value;
        }
        public override string Method {
            get => httpWebRequest.Method;
            set => httpWebRequest.Method = value;
        }
        public override WebHeaderCollection Headers {
            get => httpWebRequest.Headers;
            set => httpWebRequest.Headers = value;
        }
        public override Uri RequestUri => httpWebRequest.RequestUri;
        public override ICredentials Credentials {
            get => httpWebRequest.Credentials;
            set => httpWebRequest.Credentials = value;
        }
        public override string ContentType {
            get => httpWebRequest.ContentType;
            set => httpWebRequest.ContentType = value;
        }
        public override long ContentLength {
            get => httpWebRequest.ContentLength;
            set => httpWebRequest.ContentLength = value;
        }
        public override string ConnectionGroupName {
            get => httpWebRequest.ConnectionGroupName;
            set => httpWebRequest.ConnectionGroupName = value;
        }
        public override RequestCachePolicy CachePolicy {
            get => httpWebRequest.CachePolicy;
            set => httpWebRequest.CachePolicy = value;
        }
        public override IWebProxy Proxy {
            get => httpWebRequest.Proxy;
            set => httpWebRequest.Proxy = value;
        }
        public override int Timeout {
            get => httpWebRequest.Timeout;
            set => httpWebRequest.Timeout = value;
        }
        public override bool UseDefaultCredentials {
            get => httpWebRequest.UseDefaultCredentials;
            set => httpWebRequest.UseDefaultCredentials = value;
        }
        public override void Abort() => httpWebRequest.Abort();
        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state) => httpWebRequest.BeginGetRequestStream(callback, state);
        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state) => httpWebRequest.BeginGetResponse(callback, state);
        public override Stream EndGetRequestStream(IAsyncResult asyncResult) => httpWebRequest.EndGetRequestStream(asyncResult);
        public override WebResponse EndGetResponse(IAsyncResult asyncResult) => httpWebRequest.EndGetResponse(asyncResult);
        public override Stream GetRequestStream() => httpWebRequest.GetRequestStream();
        public override WebResponse GetResponse() => httpWebRequest.GetResponse();

        // Inherited from IHttpWebRequest

        public string Accept {
            get => httpWebRequest.Accept;
            set => httpWebRequest.Accept = value;
        }
        public string Expect {
            get => httpWebRequest.Expect;
            set => httpWebRequest.Expect = value;
        }
        public X509CertificateCollection ClientCertificates {
            get => httpWebRequest.ClientCertificates;
            set => httpWebRequest.ClientCertificates = value;
        }
        public CookieContainer CookieContainer {
            get => httpWebRequest.CookieContainer;
            set => httpWebRequest.CookieContainer = value;
        }
        public int ReadWriteTimeout {
            get => httpWebRequest.ReadWriteTimeout;
            set => httpWebRequest.ReadWriteTimeout = value;
        }
        public Uri Address => httpWebRequest.Address;
        public HttpContinueDelegate ContinueDelegate {
            get => httpWebRequest.ContinueDelegate;
            set => httpWebRequest.ContinueDelegate = value;
        }
        public ServicePoint ServicePoint => httpWebRequest.ServicePoint;
        public string Host {
            get => httpWebRequest.Host;
            set => httpWebRequest.Host = value;
        }
        public string Referer {
            get => httpWebRequest.Referer;
            set => httpWebRequest.Referer = value;
        }
        public int MaximumAutomaticRedirections {
            get => httpWebRequest.MaximumAutomaticRedirections;
            set => httpWebRequest.MaximumAutomaticRedirections = value;
        }
        public int MaximumResponseHeadersLength {
            get => httpWebRequest.MaximumResponseHeadersLength;
            set => httpWebRequest.MaximumResponseHeadersLength = value;
        }
        public Version ProtocolVersion {
            get => httpWebRequest.ProtocolVersion;
            set => httpWebRequest.ProtocolVersion = value;
        }
        public string UserAgent {
            get => httpWebRequest.UserAgent;
            set => httpWebRequest.UserAgent = value;
        }
        public string MediaType {
            get => httpWebRequest.MediaType;
            set => httpWebRequest.MediaType = value;
        }
        public string TransferEncoding {
            get => httpWebRequest.TransferEncoding;
            set => httpWebRequest.TransferEncoding = value;
        }
        public string Connection {
            get => httpWebRequest.Connection;
            set => httpWebRequest.Connection = value;
        }
        public DateTime Date {
            get => httpWebRequest.Date;
            set => httpWebRequest.Date = value;
        }
        public DecompressionMethods AutomaticDecompression {
            get => httpWebRequest.AutomaticDecompression;
            set => httpWebRequest.AutomaticDecompression = value;
        }
        public bool SendChunked {
            get => httpWebRequest.SendChunked;
            set => httpWebRequest.SendChunked = value;
        }
        public bool UnsafeAuthenticatedConnectionSharing {
            get => httpWebRequest.UnsafeAuthenticatedConnectionSharing;
            set => httpWebRequest.UnsafeAuthenticatedConnectionSharing = value;
        }
        public bool Pipelined {
            get => httpWebRequest.Pipelined;
            set => httpWebRequest.Pipelined = value;
        }
        public bool KeepAlive {
            get => httpWebRequest.KeepAlive;
            set => httpWebRequest.KeepAlive = value;
        }
        public bool HaveResponse => httpWebRequest.HaveResponse;
        public bool AllowWriteStreamBuffering {
            get => httpWebRequest.AllowWriteStreamBuffering;
            set => httpWebRequest.AllowWriteStreamBuffering = value;
        }
        public bool AllowAutoRedirect {
            get => httpWebRequest.AllowAutoRedirect;
            set => httpWebRequest.AllowAutoRedirect = value;
        }
        public DateTime IfModifiedSince {
            get => httpWebRequest.IfModifiedSince;
            set => httpWebRequest.IfModifiedSince = value;
        }

        public void AddRange(int from, int to) => httpWebRequest.AddRange(from, to);
        public void AddRange(long from, long to) => httpWebRequest.AddRange(from, to);
        public void AddRange(int range) => httpWebRequest.AddRange(range);
        public void AddRange(long range) => httpWebRequest.AddRange(range);
        public void AddRange(string rangeSpecifier, long from, long to) => httpWebRequest.AddRange(rangeSpecifier, from, to);
        public void AddRange(string rangeSpecifier, int range) => httpWebRequest.AddRange(rangeSpecifier, range);
        public void AddRange(string rangeSpecifier, long range) => httpWebRequest.AddRange(rangeSpecifier, range);
        public void AddRange(string rangeSpecifier, int from, int to) => httpWebRequest.AddRange(rangeSpecifier, from, to);

    }

}