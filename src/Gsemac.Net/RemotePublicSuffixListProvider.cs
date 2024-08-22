using Gsemac.IO.Logging;
using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using System;
using System.Net;

namespace Gsemac.Net {

    public sealed class RemotePublicSuffixListProvider :
        PublicSuffixListProviderBase {

        // Public members

        public RemotePublicSuffixListProvider() :
            this(HttpWebRequestFactory.Default) {
        }
        public RemotePublicSuffixListProvider(ILogger logger) :
            this(HttpWebRequestFactory.Default, logger) {
        }
        public RemotePublicSuffixListProvider(IHttpWebRequestFactory httpWebRequestFactory) :
            this(DefaultPublicSuffixListUri, httpWebRequestFactory) {
        }
        public RemotePublicSuffixListProvider(IHttpWebRequestFactory httpWebRequestFactory, ILogger logger) :
         this(DefaultPublicSuffixListUri, httpWebRequestFactory, logger) {
        }
        public RemotePublicSuffixListProvider(Uri uri, IHttpWebRequestFactory httpWebRequestFactory) :
            this(uri, httpWebRequestFactory, Logger.Null) {
        }
        public RemotePublicSuffixListProvider(Uri uri, IHttpWebRequestFactory httpWebRequestFactory, ILogger logger) {

            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            if (httpWebRequestFactory is null)
                throw new ArgumentNullException(nameof(httpWebRequestFactory));

            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            this.uri = uri;
            this.httpWebRequestFactory = httpWebRequestFactory;
            this.logger = new NamedLogger(logger, nameof(RemotePublicSuffixListProvider));

            // The recommended default is refreshing the list every 24 hours.
            // https://publicsuffix.org/list/

            TimeToLive = TimeSpan.FromDays(1);

        }

        public override IPublicSuffixList GetList() {

            lock (mutex) {

                bool isCacheInvalid = cache is null ||
                    (TimeToLive != default && (cacheLastUpdated == default || DateTimeOffset.Now > cacheLastUpdated + TimeToLive));

                if (isCacheInvalid) {

                    cache = GetListInternal();
                    cacheLastUpdated = DateTimeOffset.Now;

                }

                return cache;

            }

        }

        // Private members

        private readonly Uri uri;
        private readonly IHttpWebRequestFactory httpWebRequestFactory;
        private readonly ILogger logger;
        private readonly object mutex = new object();
        private IPublicSuffixList cache;
        private DateTimeOffset cacheLastUpdated;

        private static readonly Uri DefaultPublicSuffixListUri = new Uri("https://publicsuffix.org/list/public_suffix_list.dat");

        private IPublicSuffixList GetListInternal() {

            try {

                using (IWebClient webClient = httpWebRequestFactory.CreateWebClient()) {

                    string publicSuffixListStr = webClient.DownloadString(uri);

                    return new PublicSuffixList(ParseList(publicSuffixListStr));

                }

            }
            catch (WebException ex) {

                if (!FallbackEnabled)
                    throw;

                logger.Error(ex);
                logger.Warning($"An exception occurred while downloading the public suffix list from {uri}, using internal list");

            }

            // Return the default list if we weren't able to open a file.

            return base.GetList();

        }

    }

}