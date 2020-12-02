using Gsemac.Net.Extensions;
using System;
using System.Net;

namespace Gsemac.Net {

    public abstract class WebClientBase :
        WebClient {

        // Protected members

        protected WebClientBase(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

            // The Proxy property is non-null by default, and we want to know if the user set the proxy to a blank proxy intentionally.
            // Instead of trying to figure that out, we'll set the Proxy property here, and assume that whatever the property is from hereon out is what the user wants.

            this.Proxy = webRequestFactory.Options.Proxy;

        }

        protected override WebRequest GetWebRequest(Uri address) {

            IHttpWebRequest httpWebRequest = webRequestFactory.CreateHttpWebRequest(address);

            if (!(Credentials is null))
                httpWebRequest.Credentials = Credentials;

            if (!(Headers is null))
                Headers.CopyTo(httpWebRequest.Headers);

            httpWebRequest.Proxy = Proxy;

            return httpWebRequest as WebRequest;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

    }

}