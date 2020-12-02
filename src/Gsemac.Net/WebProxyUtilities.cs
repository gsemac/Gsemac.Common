using System;
using System.Net;

namespace Gsemac.Net {

    public static class WebProxyUtilities {

        public static IWebProxy GetDefaultProxy() {

            try {

                // It's possible for this to throw an exception if the user has an invalid default proxy.
                // https://stackoverflow.com/questions/13526917/error-creating-the-web-proxy-specified-in-the-system-net-defaultproxy-configur

                return WebRequest.DefaultWebProxy;

            }
            catch (Exception) {

                return new WebProxy();

            }

        }

    }

}