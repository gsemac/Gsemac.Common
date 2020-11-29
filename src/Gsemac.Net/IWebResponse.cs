using System;
using System.IO;
using System.Net;

namespace Gsemac.Net {

    public interface IWebResponse {

        /// <summary>
        /// Gets a <see cref="bool"/> value that indicates whether this response was obtained from the cache.
        /// </summary>
        bool IsFromCache { get; }
        /// <summary>
        /// Gets a <see cref="bool"/> value that indicates whether mutual authentication occurred.
        /// </summary>
        bool IsMutuallyAuthenticated { get; }
        /// <summary>
        /// When overridden in a descendant class, gets or sets the content length of data being received.
        /// </summary>
        long ContentLength { get; set; }
        /// <summary>
        ///  When overridden in a derived class, gets or sets the content type of the data being received.
        /// </summary>
        string ContentType { get; set; }
        /// <summary>
        /// When overridden in a derived class, gets the URI of the Internet resource that actually responded to the request.
        /// </summary>
        Uri ResponseUri { get; }
        /// <summary>
        ///  When overridden in a derived class, gets a collection of header name-value pairs associated with this request.
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <summary>
        /// When overridden by a descendant class, closes the response stream.
        /// </summary>
        void Close();
        /// <summary>
        /// When overridden in a descendant class, returns the data stream from the Internet resource.
        /// </summary>
        Stream GetResponseStream();

    }

}