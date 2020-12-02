using Gsemac.Net.Extensions;
using System;
using System.Net;

namespace Gsemac.Net {

    public abstract class WebClientBase :
        WebClient {

        // Protected members

        protected WebClientBase(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }

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

}