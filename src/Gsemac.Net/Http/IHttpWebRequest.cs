using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Gsemac.Net.Http {

    public interface IHttpWebRequest :
        IWebRequest {

        /// <inheritdoc cref="HttpWebRequest.Accept"/>
        string Accept { get; set; }
        /// <inheritdoc cref="HttpWebRequest.Expect"/>
        string Expect { get; set; }
        /// <inheritdoc cref="HttpWebRequest.ClientCertificates"/>
        X509CertificateCollection ClientCertificates { get; set; }
        /// <inheritdoc cref="HttpWebRequest.CookieContainer"/>
        CookieContainer CookieContainer { get; set; }
        /// <inheritdoc cref="HttpWebRequest.ReadWriteTimeout"/>
        int ReadWriteTimeout { get; set; }
        /// <inheritdoc cref="HttpWebRequest.Address"/>
        Uri Address { get; }
        /// <inheritdoc cref="HttpWebRequest.ContinueDelegate"/>
        HttpContinueDelegate ContinueDelegate { get; set; }
        /// <inheritdoc cref="HttpWebRequest.ServicePoint"/>
        ServicePoint ServicePoint { get; }
        /// <inheritdoc cref="HttpWebRequest.Host"/>
        string Host { get; set; }
        /// <inheritdoc cref="HttpWebRequest.Referer"/>
        string Referer { get; set; }
        /// <inheritdoc cref="HttpWebRequest.MaximumAutomaticRedirections"/>
        int MaximumAutomaticRedirections { get; set; }
        /// <inheritdoc cref="HttpWebRequest.MaximumResponseHeadersLength"/>
        int MaximumResponseHeadersLength { get; set; }
        /// <inheritdoc cref="HttpWebRequest.ProtocolVersion"/>
        Version ProtocolVersion { get; set; }
        /// <inheritdoc cref="HttpWebRequest.UserAgent"/>
        string UserAgent { get; set; }
        /// <inheritdoc cref="HttpWebRequest.MediaType"/>
        string MediaType { get; set; }
        /// <inheritdoc cref="HttpWebRequest.TransferEncoding"/>
        string TransferEncoding { get; set; }
        /// <inheritdoc cref="HttpWebRequest.Connection"/>
        string Connection { get; set; }
        /// <inheritdoc cref="HttpWebRequest.Date"/>
        DateTime Date { get; set; }
        /// <inheritdoc cref="HttpWebRequest.AutomaticDecompression"/>
        DecompressionMethods AutomaticDecompression { get; set; }
        /// <inheritdoc cref="HttpWebRequest.SendChunked"/>
        bool SendChunked { get; set; }
        /// <inheritdoc cref="HttpWebRequest.UnsafeAuthenticatedConnectionSharing"/>
        bool UnsafeAuthenticatedConnectionSharing { get; set; }
        /// <inheritdoc cref="HttpWebRequest.Pipelined"/>
        bool Pipelined { get; set; }
        /// <inheritdoc cref="HttpWebRequest.KeepAlive"/>
        bool KeepAlive { get; set; }
        /// <inheritdoc cref="HttpWebRequest.HaveResponse"/>
        bool HaveResponse { get; }
        /// <inheritdoc cref="HttpWebRequest.AllowWriteStreamBuffering"/>
        bool AllowWriteStreamBuffering { get; set; }
        /// <inheritdoc cref="HttpWebRequest.AllowAutoRedirect"/>
        bool AllowAutoRedirect { get; set; }
        /// <inheritdoc cref="HttpWebRequest.IfModifiedSince"/>
        DateTime IfModifiedSince { get; set; }
        /// <inheritdoc cref="HttpWebRequest.AddRange(int, int)"/>
        void AddRange(int from, int to);
        /// <inheritdoc cref="HttpWebRequest.AddRange(long, long)"/>
        void AddRange(long from, long to);
        /// <inheritdoc cref="HttpWebRequest.AddRange(int)"/>
        void AddRange(int range);
        /// <inheritdoc cref="HttpWebRequest.AddRange(long)"/>
        void AddRange(long range);
        /// <inheritdoc cref="HttpWebRequest.AddRange(string, long, long)"/>
        void AddRange(string rangeSpecifier, long from, long to);
        /// <inheritdoc cref="HttpWebRequest.AddRange(string, int)"/>
        void AddRange(string rangeSpecifier, int range);
        /// <inheritdoc cref="HttpWebRequest.AddRange(string, long)"/>
        void AddRange(string rangeSpecifier, long range);
        /// <inheritdoc cref="HttpWebRequest.AddRange(string, int, int)"/>
        void AddRange(string rangeSpecifier, int from, int to);

    }

}