using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Principal;

namespace Gsemac.Net {

    public interface IWebRequest {

        /// <summary>
        /// When overridden in a descendant class, gets or sets the protocol method to use in this request.
        /// </summary>
        string Method { get; set; }
        /// <summary>
        /// Gets or sets values indicating the level of authentication and impersonation used for this request.
        /// </summary>
        AuthenticationLevel AuthenticationLevel { get; set; }
        /// <summary>
        /// Gets or sets the length of time, in milliseconds, before the request times out.
        /// </summary>
        int Timeout { get; set; }
        /// <summary>
        ///  When overridden in a descendant class, indicates whether to pre-authenticate the request.
        /// </summary>
        bool PreAuthenticate { get; set; }
        /// <summary>
        /// When overridden in a descendant class, gets or sets the network proxy to use to access this Internet resource.
        /// </summary>
        IWebProxy Proxy { get; set; }
        /// <summary>
        /// When overridden in a descendant class, gets or sets a System.Boolean value that controls whether <see cref="CredentialCache.DefaultCredentials"></see> are sent with requests.
        /// </summary>
        bool UseDefaultCredentials { get; set; }
        /// <summary>
        /// When overridden in a descendant class, gets or sets the network credentials used for authenticating the request with the Internet resource.
        /// </summary>
        ICredentials Credentials { get; set; }
        /// <summary>
        ///  When overridden in a descendant class, gets or sets the content type of the request data being sent.
        /// </summary>
        string ContentType { get; set; }
        /// <summary>
        /// When overridden in a descendant class, gets or sets the content length of the request data being sent.
        /// </summary>
        long ContentLength { get; set; }
        /// <summary>
        /// When overridden in a descendant class, gets or sets the collection of header name/value pairs associated with the request.
        /// </summary>
        WebHeaderCollection Headers { get; set; }
        /// <summary>
        /// When overridden in a descendant class, gets or sets the name of the connection group for the request.
        /// </summary>
        string ConnectionGroupName { get; set; }
        /// <summary>
        /// Gets or sets the impersonation level for the current request.
        /// </summary>
        TokenImpersonationLevel ImpersonationLevel { get; set; }
        /// <summary>
        ///  Gets or sets the cache policy for this request.
        /// </summary>
        RequestCachePolicy CachePolicy { get; set; }
        /// <summary>
        /// When overridden in a descendant class, gets the URI of the Internet resource associated with the request.
        /// </summary>
        Uri RequestUri { get; }

        /// <summary>
        /// Aborts the request.
        /// </summary>
        void Abort();
        /// <summary>
        ///  When overridden in a descendant class, provides an asynchronous version of the System.Net.WebRequest.GetRequestStream method.
        /// </summary>
        IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state);
        /// <summary>
        ///  When overridden in a descendant class, begins an asynchronous request for an Internet resource.
        /// </summary>
        IAsyncResult BeginGetResponse(AsyncCallback callback, object state);
        /// <summary>
        /// When overridden in a descendant class, returns a System.IO.Stream for writing data to the Internet resource.
        /// </summary>
        Stream EndGetRequestStream(IAsyncResult asyncResult);
        /// <summary>
        /// When overridden in a descendant class, returns a System.Net.WebResponse.
        /// </summary>
        WebResponse EndGetResponse(IAsyncResult asyncResult);
        /// <summary>
        /// When overridden in a descendant class, returns a System.IO.Stream for writing data to the Internet resource.
        /// </summary>
        Stream GetRequestStream();
        /// <summary>
        /// When overridden in a descendant class, returns a response to an Internet request.
        /// </summary>
        WebResponse GetResponse();

    }

}