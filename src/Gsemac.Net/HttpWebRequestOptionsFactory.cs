using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Net {

    public class HttpWebRequestOptionsFactory :
        IHttpWebRequestOptionsFactory {

        // Public members

        public HttpWebRequestOptionsFactory() :
            this(HttpWebRequestOptions.Default) {
        }
        public HttpWebRequestOptionsFactory(IHttpWebRequestOptions webRequestOptions) {

            if (webRequestOptions is null)
                throw new ArgumentNullException(nameof(webRequestOptions));

            this.defaultWebRequestOptions = webRequestOptions;

        }

        public IHttpWebRequestOptions Create(Uri requestUri) {

            return GetOptions(requestUri);

        }

        public void Add(Uri endpoint, IHttpWebRequestOptions options) {

            lock (webRequestOptions)
                webRequestOptions[endpoint] = options;

        }

        // Private members

        private readonly IHttpWebRequestOptions defaultWebRequestOptions;
        private readonly IDictionary<Uri, IHttpWebRequestOptions> webRequestOptions = new Dictionary<Uri, IHttpWebRequestOptions>();

        private IHttpWebRequestOptions GetOptions(Uri endpoint) {

            lock (webRequestOptions) {

                return webRequestOptions.Where(pair => endpoint.AbsoluteUri.StartsWith(pair.Key.AbsoluteUri))
                    .OrderByDescending(pair => pair.Key.AbsoluteUri.Length)
                    .Select(pair => pair.Value)
                    .FirstOrDefault() ?? defaultWebRequestOptions;

            }

        }

    }

}