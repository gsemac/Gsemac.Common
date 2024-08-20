using Gsemac.IO.Logging;
using Gsemac.Net.Http.Extensions;
using System;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Http {

    public class RetryHandler :
        HttpWebRequestHandler {

        // Public members

        public int MaximumRetries { get; set; } = DefaultMaximumRetries;
        public bool RetryPostRequests { get; set; } = true;

        public RetryHandler() :
            this(Logger.Null) {
        }
        public RetryHandler(IHttpWebRequestFactory httpWebRequestFactory) :
          this(httpWebRequestFactory, Logger.Null) {
        }
        public RetryHandler(ILogger logger) :
            this(HttpWebRequestFactory.Default, logger) {
        }
        public RetryHandler(IHttpWebRequestFactory httpWebRequestFactory, ILogger logger) {

            if (httpWebRequestFactory is null)
                throw new ArgumentNullException(nameof(httpWebRequestFactory));

            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            this.httpWebRequestFactory = httpWebRequestFactory;
            this.logger = new NamedLogger(logger, nameof(RetryHandler));

        }

        // Protected members

        protected override IHttpWebResponse GetResponse(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (MaximumRetries <= 0)
                return base.GetResponse(request, cancellationToken);

            for (int i = 0; i < MaximumRetries + 1; ++i) {

                try {

                    return base.GetResponse(request, cancellationToken);

                }
                catch (WebException ex) {

                    if (i + 1 < MaximumRetries + 1 && IsRetryable(request, ex)) {

                        logger.Info($"Got status code {GetStatusCode(ex)}, retrying ({i + 1}/{MaximumRetries}): {request.RequestUri}");

                        IHttpWebRequest newRequest = httpWebRequestFactory.Create(request.RequestUri);

                        request.CopyTo(newRequest);

                        request = newRequest;

                    }
                    else {

                        throw;

                    }

                }

            }

            // We should never actually reach this point.

            return base.GetResponse(request, cancellationToken);

        }

        // Private members

        private const int DefaultMaximumRetries = 3;

        private readonly IHttpWebRequestFactory httpWebRequestFactory;
        private readonly ILogger logger;

        private HttpStatusCode GetStatusCode(WebException ex) {

            if (ex is null)
                throw new ArgumentNullException(nameof(ex));

            return new HttpWebResponseAdapter(ex.Response).StatusCode;

        }
        private bool IsRetryable(IHttpWebRequest httpWebRequest, WebException ex) {

            if (httpWebRequest is null)
                throw new ArgumentNullException(nameof(httpWebRequest));

            if (ex is null)
                throw new ArgumentNullException(nameof(ex));

            if (!RetryPostRequests && (httpWebRequest.Method?.Equals("POST", StringComparison.OrdinalIgnoreCase) ?? false))
                return false;

            // We can always retry if a request just timed out.

            if (ex.Status == WebExceptionStatus.Timeout)
                return true;

            if (ex.Response is null)
                return false;

            // Only some status codes are considered valid for retries.
            // https://stackoverflow.com/a/74627395/5383169

            switch (GetStatusCode(ex)) {

                case HttpStatusCode.RequestTimeout:
                case (HttpStatusCode)425: // Too Early
                case (HttpStatusCode)429: // Too Many Requests
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    return true;

            }

            return false;

        }

    }

}