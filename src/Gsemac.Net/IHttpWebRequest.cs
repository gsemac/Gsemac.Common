using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Gsemac.Net {

    public interface IHttpWebRequest :
        IWebRequest {

        /// <summary>
        /// Gets or sets the value of the Accept HTTP header.
        /// </summary>
        string Accept { get; set; }
        /// <summary>
        /// Gets or sets the value of the Expect HTTP header.
        /// </summary>
        string Expect { get; set; }
        /// <summary>
        /// Gets or sets the collection of security certificates that are associated with this request.
        /// </summary>
        X509CertificateCollection ClientCertificates { get; set; }
        /// <summary>
        /// Gets or sets the cookies associated with the request.
        /// </summary>
        CookieContainer CookieContainer { get; set; }
        /// <summary>
        /// Gets or sets a time-out in milliseconds when writing to or reading from a stream.
        /// </summary>
        int ReadWriteTimeout { get; set; }
        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the Internet resource that actually responds to the request.
        /// </summary>
        Uri Address { get; }
        /// <summary>
        ///  Gets or sets the delegate method called when an HTTP 100-continue response is received from the Internet resource.
        /// </summary>
        HttpContinueDelegate ContinueDelegate { get; set; }
        /// <summary>
        /// Gets the service point to use for the request.
        /// </summary>
        ServicePoint ServicePoint { get; }
        /// <summary>
        ///  Get or set the Host header value to use in an HTTP request independent from the request URI.
        /// </summary>
        string Host { get; set; }
        /// <summary>
        /// Gets or sets the value of the Referer HTTP header.
        /// </summary>
        string Referer { get; set; }
        /// <summary>
        ///  Gets or sets the maximum number of redirects that the request follows.
        /// </summary>
        int MaximumAutomaticRedirections { get; set; }
        /// <summary>
        /// Gets or sets the maximum allowed length of the response headers.
        /// </summary>
        int MaximumResponseHeadersLength { get; set; }
        /// <summary>
        /// Gets or sets the version of HTTP to use for the request.
        /// </summary>
        Version ProtocolVersion { get; set; }
        /// <summary>
        /// Gets or sets the value of the User-agent HTTP header.
        /// </summary>
        string UserAgent { get; set; }
        /// <summary>
        /// Gets or sets the media type of the request.
        /// </summary>
        string MediaType { get; set; }
        /// <summary>
        /// Gets or sets the value of the Transfer-encoding HTTP header.
        /// </summary>
        string TransferEncoding { get; set; }
        /// <summary>
        /// Gets or sets the value of the Connection HTTP header.
        /// </summary>
        string Connection { get; set; }
        /// <summary>
        /// Get or set the Date HTTP header value to use in an HTTP request.
        /// </summary>
        DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the type of decompression that is used.
        /// </summary>
        DecompressionMethods AutomaticDecompression { get; set; }
        /// <summary>
        ///  Gets or sets a value that indicates whether to send data in segments to the Internet resource.
        /// </summary>
        bool SendChunked { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether to allow high-speed NTLM-authenticated connection sharing.
        /// </summary>
        bool UnsafeAuthenticatedConnectionSharing { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether to pipeline the request to the Internet resource.
        /// </summary>
        bool Pipelined { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.
        /// </summary>
        bool KeepAlive { get; set; }
        /// <summary>
        ///  Gets a value that indicates whether a response has been received from an Internet resource.
        /// </summary>
        bool HaveResponse { get; }
        /// <summary>
        /// Gets or sets a value that indicates whether to buffer the data sent to the Internet resource.
        /// </summary>
        bool AllowWriteStreamBuffering { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether the request should follow redirection responses.
        /// </summary>
        bool AllowAutoRedirect { get; set; }
        /// <summary>
        /// Gets or sets the value of the If-Modified-Since HTTP header.
        /// </summary>
        DateTime IfModifiedSince { get; set; }
        /// <summary>
        /// Adds a byte range header to the request for a specified range.
        /// </summary>
        /// <param name="from">The position at which to start sending data.</param>
        /// <param name="to">The position at which to stop sending data.</param>
        void AddRange(int from, int to);
        /// <summary>
        /// Adds a byte range header to the request for a specified range.
        /// </summary>
        /// <param name="from">The position at which to start sending data.</param>
        /// <param name="to">The position at which to stop sending data.</param>
        void AddRange(long from, long to);
        /// <summary>
        /// Adds a byte range header to a request for a specific range from the beginning or end of the requested data.
        /// </summary>
        /// <param name="range">The starting or ending point of the range.</param>
        void AddRange(int range);
        /// <summary>
        /// Adds a byte range header to a request for a specific range from the beginning or end of the requested data.
        /// </summary>
        /// <param name="range">The starting or ending point of the range.</param>
        void AddRange(long range);
        /// <summary>
        /// Adds a range header to a request for a specified range.
        /// </summary>
        /// <param name="rangeSpecifier"></param>
        /// <param name="from">The position at which to start sending data.</param>
        /// <param name="to">The position at which to stop sending data.</param>
        void AddRange(string rangeSpecifier, long from, long to);
        /// <summary>
        /// Adds a range header to a request for a specified range.
        /// </summary>
        /// <param name="rangeSpecifier"></param>
        /// <param name="from">The position at which to start sending data.</param>
        /// <param name="to">The position at which to stop sending data.</param>
        void AddRange(string rangeSpecifier, int range);
        /// <summary>
        /// Adds a range header to a request for a specified range.
        /// </summary>
        /// <param name="rangeSpecifier"></param>
        /// <param name="from">The position at which to start sending data.</param>
        /// <param name="to">The position at which to stop sending data.</param>
        void AddRange(string rangeSpecifier, long range);
        /// <summary>
        /// Adds a range header to a request for a specified range.
        /// </summary>
        /// <param name="rangeSpecifier"></param>
        /// <param name="from">The position at which to start sending data.</param>
        /// <param name="to">The position at which to stop sending data.</param>
        void AddRange(string rangeSpecifier, int from, int to);

    }

}