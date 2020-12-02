using Gsemac.Net.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gsemac.Net {

    public class WebClientFactory :
        IWebClientFactory {

        // Public members

        public IHttpWebRequestOptions Options {
            get => webRequestFactory.Options;
            set => webRequestFactory.Options = value;
        }

        public WebClientFactory(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }

        public WebClient CreateWebClient() {

            return new FactorylWebClient(webRequestFactory);

        }

        // Private members

        private class FactorylWebClient :
            WebClient {

            // Public members

            public FactorylWebClient(IHttpWebRequestFactory webRequestFactory) {

                this.webRequestFactory = webRequestFactory;

            }

            // Protected members

            protected override WebRequest GetWebRequest(Uri address) {

                IHttpWebRequest httpWebRequest = webRequestFactory.CreateHttpWebRequest(address);

                if (!(Credentials is null))
                    httpWebRequest.Credentials = Credentials;

                if (!(Proxy is null))
                    httpWebRequest.Proxy = Proxy;

                if (!(Headers is null))
                    Headers.CopyTo(httpWebRequest.Headers);

                return httpWebRequest as WebRequest;

            }

            // Private members

            private readonly IHttpWebRequestFactory webRequestFactory;

        }

        private readonly IHttpWebRequestFactory webRequestFactory;

    }

}