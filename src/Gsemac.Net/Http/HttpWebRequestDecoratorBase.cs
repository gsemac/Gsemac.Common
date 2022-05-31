using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Gsemac.Net.Http {

    public abstract class HttpWebRequestDecoratorBase :
        WebRequest,
        IHttpWebRequest {

        // Internal members

        internal WebRequest InnerWebRequest => (WebRequest)innerHttpWebRequest;

        // Protected members

        protected HttpWebRequestDecoratorBase(IHttpWebRequest innerHttpWebRequest) {

            if (innerHttpWebRequest is null)
                throw new ArgumentNullException(nameof(innerHttpWebRequest));

            this.innerHttpWebRequest = innerHttpWebRequest;

        }
        protected HttpWebRequestDecoratorBase(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

        // Private members

        private readonly IHttpWebRequest innerHttpWebRequest;

        // Inherited from WebRequest

        public override bool PreAuthenticate {
            get => innerHttpWebRequest.PreAuthenticate;
            set => innerHttpWebRequest.PreAuthenticate = value;
        }
        public override string Method {
            get => innerHttpWebRequest.Method;
            set => innerHttpWebRequest.Method = value;
        }
        public override WebHeaderCollection Headers {
            get => innerHttpWebRequest.Headers;
            set => innerHttpWebRequest.Headers = value;
        }
        public override Uri RequestUri => innerHttpWebRequest.RequestUri;
        public override ICredentials Credentials {
            get => innerHttpWebRequest.Credentials;
            set => innerHttpWebRequest.Credentials = value;
        }
        public override string ContentType {
            get => innerHttpWebRequest.ContentType;
            set => innerHttpWebRequest.ContentType = value;
        }
        public override long ContentLength {
            get => innerHttpWebRequest.ContentLength;
            set => innerHttpWebRequest.ContentLength = value;
        }
        public override string ConnectionGroupName {
            get => innerHttpWebRequest.ConnectionGroupName;
            set => innerHttpWebRequest.ConnectionGroupName = value;
        }
        public override RequestCachePolicy CachePolicy {
            get => innerHttpWebRequest.CachePolicy;
            set => innerHttpWebRequest.CachePolicy = value;
        }
        public override IWebProxy Proxy {
            get => innerHttpWebRequest.Proxy;
            set => innerHttpWebRequest.Proxy = value;
        }
        public override int Timeout {
            get => innerHttpWebRequest.Timeout;
            set => innerHttpWebRequest.Timeout = value;
        }
        public override bool UseDefaultCredentials {
            get => innerHttpWebRequest.UseDefaultCredentials;
            set => innerHttpWebRequest.UseDefaultCredentials = value;
        }

        public new AuthenticationLevel AuthenticationLevel {
            get => base.AuthenticationLevel;
            set => base.AuthenticationLevel = value;
        }
        public new TokenImpersonationLevel ImpersonationLevel {
            get => base.ImpersonationLevel;
            set => base.ImpersonationLevel = value;
        }

        public override void Abort() => innerHttpWebRequest.Abort();
        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state) => innerHttpWebRequest.BeginGetRequestStream(callback, state);
        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state) => innerHttpWebRequest.BeginGetResponse(callback, state);
        public override Stream EndGetRequestStream(IAsyncResult asyncResult) => innerHttpWebRequest.EndGetRequestStream(asyncResult);
        public override WebResponse EndGetResponse(IAsyncResult asyncResult) => innerHttpWebRequest.EndGetResponse(asyncResult);
        public override Stream GetRequestStream() => innerHttpWebRequest.GetRequestStream();
        public override WebResponse GetResponse() => innerHttpWebRequest.GetResponse();

        // Inherited from IHttpWebRequest

        public virtual string Accept {
            get => innerHttpWebRequest.Accept;
            set => innerHttpWebRequest.Accept = value;
        }
        public virtual string Expect {
            get => innerHttpWebRequest.Expect;
            set => innerHttpWebRequest.Expect = value;
        }
        public virtual X509CertificateCollection ClientCertificates {
            get => innerHttpWebRequest.ClientCertificates;
            set => innerHttpWebRequest.ClientCertificates = value;
        }
        public virtual CookieContainer CookieContainer {
            get => innerHttpWebRequest.CookieContainer;
            set => innerHttpWebRequest.CookieContainer = value;
        }
        public virtual int ReadWriteTimeout {
            get => innerHttpWebRequest.ReadWriteTimeout;
            set => innerHttpWebRequest.ReadWriteTimeout = value;
        }
        public virtual Uri Address => innerHttpWebRequest.Address;
        public virtual HttpContinueDelegate ContinueDelegate {
            get => innerHttpWebRequest.ContinueDelegate;
            set => innerHttpWebRequest.ContinueDelegate = value;
        }
        public virtual ServicePoint ServicePoint => innerHttpWebRequest.ServicePoint;
        public virtual string Host {
            get => innerHttpWebRequest.Host;
            set => innerHttpWebRequest.Host = value;
        }
        public virtual string Referer {
            get => innerHttpWebRequest.Referer;
            set => innerHttpWebRequest.Referer = value;
        }
        public virtual int MaximumAutomaticRedirections {
            get => innerHttpWebRequest.MaximumAutomaticRedirections;
            set => innerHttpWebRequest.MaximumAutomaticRedirections = value;
        }
        public virtual int MaximumResponseHeadersLength {
            get => innerHttpWebRequest.MaximumResponseHeadersLength;
            set => innerHttpWebRequest.MaximumResponseHeadersLength = value;
        }
        public virtual Version ProtocolVersion {
            get => innerHttpWebRequest.ProtocolVersion;
            set => innerHttpWebRequest.ProtocolVersion = value;
        }
        public virtual string UserAgent {
            get => innerHttpWebRequest.UserAgent;
            set => innerHttpWebRequest.UserAgent = value;
        }
        public virtual string MediaType {
            get => innerHttpWebRequest.MediaType;
            set => innerHttpWebRequest.MediaType = value;
        }
        public virtual string TransferEncoding {
            get => innerHttpWebRequest.TransferEncoding;
            set => innerHttpWebRequest.TransferEncoding = value;
        }
        public virtual string Connection {
            get => innerHttpWebRequest.Connection;
            set => innerHttpWebRequest.Connection = value;
        }
        public virtual DateTime Date {
            get => innerHttpWebRequest.Date;
            set => innerHttpWebRequest.Date = value;
        }
        public virtual DecompressionMethods AutomaticDecompression {
            get => innerHttpWebRequest.AutomaticDecompression;
            set => innerHttpWebRequest.AutomaticDecompression = value;
        }
        public virtual bool SendChunked {
            get => innerHttpWebRequest.SendChunked;
            set => innerHttpWebRequest.SendChunked = value;
        }
        public virtual bool UnsafeAuthenticatedConnectionSharing {
            get => innerHttpWebRequest.UnsafeAuthenticatedConnectionSharing;
            set => innerHttpWebRequest.UnsafeAuthenticatedConnectionSharing = value;
        }
        public virtual bool Pipelined {
            get => innerHttpWebRequest.Pipelined;
            set => innerHttpWebRequest.Pipelined = value;
        }
        public virtual bool KeepAlive {
            get => innerHttpWebRequest.KeepAlive;
            set => innerHttpWebRequest.KeepAlive = value;
        }
        public virtual bool HaveResponse => innerHttpWebRequest.HaveResponse;
        public virtual bool AllowWriteStreamBuffering {
            get => innerHttpWebRequest.AllowWriteStreamBuffering;
            set => innerHttpWebRequest.AllowWriteStreamBuffering = value;
        }
        public virtual bool AllowAutoRedirect {
            get => innerHttpWebRequest.AllowAutoRedirect;
            set => innerHttpWebRequest.AllowAutoRedirect = value;
        }
        public virtual DateTime IfModifiedSince {
            get => innerHttpWebRequest.IfModifiedSince;
            set => innerHttpWebRequest.IfModifiedSince = value;
        }

        public virtual void AddRange(int from, int to) => innerHttpWebRequest.AddRange(from, to);
        public virtual void AddRange(long from, long to) => innerHttpWebRequest.AddRange(from, to);
        public virtual void AddRange(int range) => innerHttpWebRequest.AddRange(range);
        public virtual void AddRange(long range) => innerHttpWebRequest.AddRange(range);
        public virtual void AddRange(string rangeSpecifier, long from, long to) => innerHttpWebRequest.AddRange(rangeSpecifier, from, to);
        public virtual void AddRange(string rangeSpecifier, int range) => innerHttpWebRequest.AddRange(rangeSpecifier, range);
        public virtual void AddRange(string rangeSpecifier, long range) => innerHttpWebRequest.AddRange(rangeSpecifier, range);
        public virtual void AddRange(string rangeSpecifier, int from, int to) => innerHttpWebRequest.AddRange(rangeSpecifier, from, to);

    }

}