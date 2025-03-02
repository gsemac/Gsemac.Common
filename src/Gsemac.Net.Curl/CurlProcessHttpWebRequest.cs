using Gsemac.IO.Extensions;
using Gsemac.Net.Http;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Gsemac.Net.Curl {

    public class CurlProcessHttpWebRequest :
        HttpWebRequestBase {

        // Public members

        /// <summary>
        /// Returns the arguments that are passed to curl.
        /// </summary>
        public string CurlArguments => GetCurlArguments();

        public CurlProcessHttpWebRequest(Uri requestUri, ICurlWebRequestOptions options = null) :
            base(requestUri) {

            this.options = options ?? CurlWebRequestOptions.Default;

        }
        public CurlProcessHttpWebRequest(string requestUri, ICurlWebRequestOptions options = null) :
           this(new Uri(requestUri), options) {
        }

        // Methods overidden from WebRequest

        public override WebResponse GetResponse() {

            CurlProcessStream stream = new CurlProcessStream(options.CurlExecutablePath, CurlArguments) {
                ReadTimeout = ReadWriteTimeout,
                WriteTimeout = ReadWriteTimeout
            };

            HaveResponse = true;

            return new CurlProcessHttpWebResponse(this, stream);

        }

        // Private members

        private readonly ICurlWebRequestOptions options;

        private string GetPostData() {

            Stream requestStream = GetRequestStream(validateMethod: false);

            if (requestStream.Length <= 0)
                return string.Empty;

            return Encoding.UTF8.GetString(requestStream.ToArray());

        }
        private string GetCurlArguments() {

            return new CurlProcessArgumentsBuilder()
                .WithHeaderOutput()
                .WithConsoleOutput()
                .WithAutomaticRedirect(AllowAutoRedirect ? MaximumAutomaticRedirections : 0)
                .WithAutomaticDecompression(AutomaticDecompression)
                .WithCertificateValidation(ServicePointManagerUtilities.IsCertificateValidationEnabled())
                .WithConnectTimeout(Timeout)
                .WithCookies(CookieContainer, RequestUri)
                .WithCredentials(Credentials, RequestUri)
                .WithHeaders(Headers)
                .WithHttpVersion(ProtocolVersion)
                .WithKeepAlive(KeepAlive)
                .WithPostData(GetPostData())
                .WithProxy(Proxy, RequestUri)
                .WithUri(RequestUri)
                .ToString();

        }

    }

}