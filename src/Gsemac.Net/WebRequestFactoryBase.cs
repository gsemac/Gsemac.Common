using Gsemac.Net.Http;
using System;
using System.Net;

namespace Gsemac.Net {

    public abstract class WebRequestFactoryBase :
        IWebRequestFactory {

        // Public members

        public virtual IWebRequest Create(Uri requestUri) {

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            WebRequest request = WebRequest.Create(requestUri);

            // By returning a HttpWebRequestAdapter for HttpWebRequest instances, the user can cast to IHttpWebRequest.

            if (request is HttpWebRequest httpWebRequest)
                return new HttpWebRequestAdapter(httpWebRequest);
            else
                return new WebRequestAdapter(request);

        }

    }

}