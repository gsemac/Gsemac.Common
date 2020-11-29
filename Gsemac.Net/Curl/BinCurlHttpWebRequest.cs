﻿using Gsemac.IO.Extensions;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Gsemac.Net.Curl {

    public class BinCurlHttpWebRequest :
        HttpWebRequestBase {

        // Public members

        /// <summary>
        /// Returns the arguments that are passed to curl.
        /// </summary>
        public string CurlArguments => GetCurlArguments();

        public BinCurlHttpWebRequest(Uri requestUri) :
            this(requestUri, LibCurl.CurlExecutablePath) {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BinCurlHttpWebRequest"/> class.
        /// </summary>
        /// <param name="curlExecutablePath">Path to the curl executable.</param>
        /// <param name="requestUri">URI to request.</param>
        public BinCurlHttpWebRequest(Uri requestUri, string curlExecutablePath) :
            base(requestUri) {

            this.curlExecutablePath = curlExecutablePath;

        }

        // Methods overidden from WebRequest

        public override WebResponse GetResponse() {

            BinCurlProcessStream stream = new BinCurlProcessStream(curlExecutablePath, CurlArguments) {
                ReadTimeout = ReadWriteTimeout,
                WriteTimeout = ReadWriteTimeout
            };

            HaveResponse = true;

            return new BinCurlHttpWebResponse(this, stream);

        }

        // Private members

        private readonly string curlExecutablePath;

        private string GetPostData() {

            Stream requestStream = GetRequestStream(validateMethod: false);

            if (requestStream.Length <= 0)
                return string.Empty;

            return Encoding.UTF8.GetString(requestStream.ToArray());

        }
        private string GetCurlArguments() {

            return new CurlCommandLineArgumentsBuilder()
                .WithHeaderOutput()
                .WithConsoleOutput()
                .WithAutomaticRedirect(AllowAutoRedirect ? MaximumAutomaticRedirections : 0)
                .WithAutomaticDecompression(AutomaticDecompression)
                .WithHttpVersion(ProtocolVersion)
                .WithKeepAlive(KeepAlive)
                .WithConnectTimeout(Timeout)
                .WithHeaders(Headers)
                .WithCookies(CookieContainer, RequestUri)
                .WithPostData(GetPostData())
                .WithProxy(Proxy, RequestUri)
                .WithCredentials(Credentials, RequestUri)
                .WithUri(RequestUri)
                .ToString();

        }

    }

}