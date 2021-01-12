using System;

namespace Gsemac.Net {

    public class HttpWebRequestOptionsFactory :
        IHttpWebRequestOptionsFactory {

        // Public members

        public HttpWebRequestOptionsFactory() {
        }
        public HttpWebRequestOptionsFactory(IHttpWebRequestOptions webRequestOptions) {

            if (webRequestOptions is null)
                throw new ArgumentNullException(nameof(webRequestOptions));

            this.webRequestOptions = webRequestOptions;

        }

        public IHttpWebRequestOptions Create(Uri requestUri) {

            return webRequestOptions ?? HttpWebRequestOptions.Default;

        }

        // Private members

        private readonly IHttpWebRequestOptions webRequestOptions;

    }

}