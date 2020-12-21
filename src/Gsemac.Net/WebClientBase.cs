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

            webRequestFactory.GetOptions().CopyTo(this);

        }

        protected override WebRequest GetWebRequest(Uri address) {

            // The only way to get the method is to call base.GetWebRequest() and copy its properties.
            // Note that calling this method also clears most headers from the WebClient.

            WebRequest baseWebRequest = base.GetWebRequest(address);

            if (baseWebRequest is HttpWebRequest baseHttpWebRequest) {

                IHttpWebRequest httpWebRequest = webRequestFactory.Create(address);

                httpWebRequest.Credentials = baseHttpWebRequest.Credentials;
                httpWebRequest.Method = baseHttpWebRequest.Method;
                httpWebRequest.Proxy = baseHttpWebRequest.Proxy;

                httpWebRequest.Headers.Clear();
                baseHttpWebRequest.Headers.CopyTo(httpWebRequest);

                return httpWebRequest as WebRequest;

            }
            else {

                return baseWebRequest;

            }

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

    }

}