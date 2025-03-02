using Gsemac.IO.Logging;
using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Curl {

    // Automatically retry requests made with .NET's native HTTP engine with Curl when we get a "SecureChannelFailure" error.
    // This allows requests using protocols not supported on older versions of .NET (e.g. TLS 1.3) to succeed.

    public sealed class RetryWithCurlOnSecureChannelFailureHandler :
        HttpWebRequestHandler {

        // Public members

        public RetryWithCurlOnSecureChannelFailureHandler() :
            this(Logger.Null) {
        }
        public RetryWithCurlOnSecureChannelFailureHandler(ICurlHttpWebRequestFactory webRequestFactory) :
            this(webRequestFactory, Logger.Null) {
        }
        public RetryWithCurlOnSecureChannelFailureHandler(ILogger logger) :
           this(new CurlHttpWebRequestFactory(), logger) {
        }
        public RetryWithCurlOnSecureChannelFailureHandler(ICurlHttpWebRequestFactory webRequestFactory, ILogger logger) :
            base() {

            if (webRequestFactory is null)
                throw new ArgumentNullException(nameof(webRequestFactory));

            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            this.webRequestFactory = webRequestFactory;
            this.logger = new NamedLogger(logger, nameof(RetryWithCurlOnSecureChannelFailureHandler));

        }

        // Protected members

        protected override IHttpWebResponse GetResponse(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            bool shouldHandleRequestWithCurl = false;

            lock (hostnameCache)
                shouldHandleRequestWithCurl = hostnameCache.Contains(request.Host);

            if (shouldHandleRequestWithCurl)
                return GetResponseWithCurl(request, cancellationToken);

            try {

                return base.GetResponse(request, cancellationToken);

            }
            catch (WebException ex) {

                bool isFailedToCreateSecureChannelError = ex.Status == WebExceptionStatus.SecureChannelFailure;

                if (isFailedToCreateSecureChannelError) {

                    if (HttpWebRequestUtilities.GetInnermostWebRequest((WebRequest)request) is CurlHttpWebRequest)
                        throw;

                    try {

                        logger.Error(ex);
                        logger.Warning($"Failed to create secure channel. Retrying request with Curl.");

                        IHttpWebResponse response = GetResponseWithCurl(request, cancellationToken);

                        // If the request was successful, use Curl for subsequent requests to this host.

                        lock (hostnameCache)
                            hostnameCache.Add(request.Host);

                        return response;

                    }
                    finally {

                        // Make sure to close the response before returning, because it will not be accessible to the caller.

                        if (ex.Response is object)
                            ex.Response.Close();

                    }

                }
                else {

                    throw;

                }

            }

        }

        // Private members

        private readonly ILogger logger;
        private readonly ICurlHttpWebRequestFactory webRequestFactory;
        private static readonly HashSet<string> hostnameCache = new HashSet<string>();

        private IHttpWebResponse GetResponseWithCurl(IHttpWebRequest request, CancellationToken cancellationToken) {

            IHttpWebRequest curlWebRequest = webRequestFactory.Create(request.RequestUri)
                .WithOptions(HttpWebRequestOptions.FromHttpWebRequest(request));

            return base.GetResponse(curlWebRequest, cancellationToken);

        }

    }

}