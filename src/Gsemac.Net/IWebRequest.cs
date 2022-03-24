using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Principal;

namespace Gsemac.Net {

    public interface IWebRequest {

        /// <inheritdoc cref="WebRequest.Method"/>
        string Method { get; set; }
        /// <inheritdoc cref="WebRequest.AuthenticationLevel"/>
        AuthenticationLevel AuthenticationLevel { get; set; }
        /// <inheritdoc cref="WebRequest.Timeout"/>
        int Timeout { get; set; }
        /// <inheritdoc cref="WebRequest.PreAuthenticate"/>
        bool PreAuthenticate { get; set; }
        /// <inheritdoc cref="WebRequest.Proxy"/>
        IWebProxy Proxy { get; set; }
        /// <inheritdoc cref="WebRequest.UseDefaultCredentials"/>
        bool UseDefaultCredentials { get; set; }
        /// <inheritdoc cref="WebRequest.Credentials"/>
        ICredentials Credentials { get; set; }
        /// <inheritdoc cref="WebRequest.ContentType"/>
        string ContentType { get; set; }
        /// <inheritdoc cref="WebRequest.ContentLength"/>
        long ContentLength { get; set; }
        /// <inheritdoc cref="WebRequest.Headers"/>
        WebHeaderCollection Headers { get; set; }
        /// <inheritdoc cref="WebRequest.ConnectionGroupName"/>
        string ConnectionGroupName { get; set; }
        /// <inheritdoc cref="WebRequest.ImpersonationLevel"/>
        TokenImpersonationLevel ImpersonationLevel { get; set; }
        /// <inheritdoc cref="WebRequest.CachePolicy"/>
        RequestCachePolicy CachePolicy { get; set; }
        /// <inheritdoc cref="WebRequest.RequestUri"/>
        Uri RequestUri { get; }

        /// <inheritdoc cref="WebRequest.Abort"/>
        void Abort();
        /// <inheritdoc cref="WebRequest.BeginGetRequestStream"/>
        IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state);
        /// <inheritdoc cref="WebRequest.BeginGetResponse"/>
        IAsyncResult BeginGetResponse(AsyncCallback callback, object state);
        /// <inheritdoc cref="WebRequest.EndGetRequestStream"/>
        Stream EndGetRequestStream(IAsyncResult asyncResult);
        /// <inheritdoc cref="WebRequest.EndGetResponse"/>
        WebResponse EndGetResponse(IAsyncResult asyncResult);
        /// <inheritdoc cref="WebRequest.GetRequestStream"/>
        Stream GetRequestStream();
        /// <inheritdoc cref="WebRequest.GetResponse"/>
        WebResponse GetResponse();

    }

}