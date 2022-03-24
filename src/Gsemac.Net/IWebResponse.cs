using System;
using System.IO;
using System.Net;

namespace Gsemac.Net {

    public interface IWebResponse {

        /// <inheritdoc cref="WebResponse.IsFromCache"/>
        bool IsFromCache { get; }
        /// <inheritdoc cref="WebResponse.IsMutuallyAuthenticated"/>
        bool IsMutuallyAuthenticated { get; }
        /// <inheritdoc cref="WebResponse.ContentLength"/>
        long ContentLength { get; set; }
        /// <inheritdoc cref="WebResponse.ContentType"/>
        string ContentType { get; set; }
        /// <inheritdoc cref="WebResponse.ResponseUri"/>
        Uri ResponseUri { get; }
        /// <inheritdoc cref="WebResponse.Headers"/>
        WebHeaderCollection Headers { get; }

        /// <inheritdoc cref="WebResponse.Close"/>
        void Close();
        /// <inheritdoc cref="WebResponse.GetResponseStream"/>
        Stream GetResponseStream();

    }

}