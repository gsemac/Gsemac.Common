using System;
using System.Net;

namespace Gsemac.Net {

    public interface IHttpWebResponse {

        /// <summary>
        /// Gets the method that is used to encode the body of the response.
        /// </summary>
        string ContentEncoding { get; }
        /// <summary>
        /// Gets the version of the HTTP protocol that is used in the response.
        /// </summary>
        Version ProtocolVersion { get; }
        /// <summary>
        /// Gets the status description returned with the response.
        /// </summary>
        string StatusDescription { get; }
        /// <summary>
        /// Gets the status of the response.
        /// </summary>
        HttpStatusCode StatusCode { get; }
        /// <summary>
        /// Gets the last date and time that the contents of the response were modified.
        /// </summary>
        DateTime LastModified { get; }
        /// <summary>
        /// Gets the name of the server that sent the response.
        /// </summary>
        string Server { get; }
        /// <summary>
        /// Gets the character set of the response.
        /// </summary>
        string CharacterSet { get; }
        /// <summary>
        /// Gets the method that is used to return the response.
        /// </summary>
        string Method { get; }
        /// <summary>
        /// Gets or sets the cookies that are associated with this response.
        /// </summary>
        CookieCollection Cookies { get; set; }
        /// <summary>
        /// Gets the contents of a header that was returned with the response.
        /// </summary>
        /// <param name="headerName">The header value to return.</param>
        /// <returns>The contents of the specified header.</returns>
        string GetResponseHeader(string headerName);

    }

}