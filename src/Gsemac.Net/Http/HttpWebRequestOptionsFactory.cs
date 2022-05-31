using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.Http {

    public class HttpWebRequestOptionsFactory :
        IHttpWebRequestOptionsFactory {

        // Public members

        public HttpWebRequestOptionsFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public HttpWebRequestOptionsFactory(IHttpWebRequestOptions webRequestOptions) {

            if (webRequestOptions is null)
                throw new ArgumentNullException(nameof(webRequestOptions));

            defaultWebRequestOptions = webRequestOptions;

        }

        public IHttpWebRequestOptions Create() {

            return defaultWebRequestOptions;

        }
        public IHttpWebRequestOptions Create(Uri requestUri) {

            return GetOptions(requestUri);

        }

        public void Add(Uri endpoint, IHttpWebRequestOptions options, bool copyIfNull = true) {

            lock (webRequestOptions)
                webRequestOptions[endpoint] = new OptionsInfo(options, copyIfNull);

        }

        // Private members

        private class OptionsInfo {

            public IHttpWebRequestOptions WebRequestOptions { get; }
            public bool CopyIfNull { get; }

            public OptionsInfo(IHttpWebRequestOptions webRequestOptions, bool copyIfNull) {

                WebRequestOptions = webRequestOptions;
                CopyIfNull = copyIfNull;

            }

        }

        private readonly IHttpWebRequestOptions defaultWebRequestOptions;
        private readonly IDictionary<Uri, OptionsInfo> webRequestOptions = new Dictionary<Uri, OptionsInfo>();

        private IHttpWebRequestOptions GetOptions(Uri endpoint) {

            lock (webRequestOptions) {

                OptionsInfo optionsInfo = webRequestOptions.Where(pair => endpoint.AbsoluteUri.StartsWith(pair.Key.AbsoluteUri))
                    .OrderByDescending(pair => pair.Key.AbsoluteUri.Length)
                    .Select(pair => pair.Value)
                    .FirstOrDefault();

                if (optionsInfo is null)
                    return defaultWebRequestOptions;

                return HttpWebRequestOptions.Combine(defaultWebRequestOptions, optionsInfo.WebRequestOptions, optionsInfo.CopyIfNull);

            }

        }

    }

}