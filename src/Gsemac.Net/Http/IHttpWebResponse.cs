using System;
using System.Net;

namespace Gsemac.Net.Http {

    public interface IHttpWebResponse :
        IWebResponse {

        /// <inheritdoc cref="HttpWebResponse.ContentEncoding"/>
        string ContentEncoding { get; }
        /// <inheritdoc cref="HttpWebResponse.ProtocolVersion"/>
        Version ProtocolVersion { get; }
        /// <inheritdoc cref="HttpWebResponse.StatusDescription"/>
        string StatusDescription { get; }
        /// <inheritdoc cref="HttpWebResponse.StatusCode"/>
        HttpStatusCode StatusCode { get; }
        /// <inheritdoc cref="HttpWebResponse.LastModified"/>
        DateTime LastModified { get; }
        /// <inheritdoc cref="HttpWebResponse.Server"/>
        string Server { get; }
        /// <inheritdoc cref="HttpWebResponse.CharacterSet"/>
        string CharacterSet { get; }
        /// <inheritdoc cref="HttpWebResponse.Method"/>
        string Method { get; }
        /// <inheritdoc cref="HttpWebResponse.Cookies"/>
        CookieCollection Cookies { get; set; }
        /// <inheritdoc cref="HttpWebResponse.GetResponseHeader"/>
        string GetResponseHeader(string headerName);

    }

}