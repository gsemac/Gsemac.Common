using System;
using System.Net;
using System.Reflection;

namespace Gsemac.Net {

    public static class WebClientUtilities {

        // Public members

        public static WebRequest GetWebRequest(WebClient webClient, Uri address) {

            MethodInfo getWebRequestMethod = typeof(WebClient)
                .GetMethod("GetWebRequest", BindingFlags.NonPublic | BindingFlags.Instance);

            return (WebRequest)getWebRequestMethod.Invoke(webClient, new object[] { address });

        }
        public static WebResponse GetWebResponse(WebClient webClient, WebRequest webRequest) {

            MethodInfo getWebRequestMethod = typeof(WebClient)
                .GetMethod("GetWebResponse", BindingFlags.NonPublic | BindingFlags.Instance);

            return (WebResponse)getWebRequestMethod.Invoke(webClient, new object[] { webRequest });

        }

        // Internal members

        internal static WebClient GetInnermostWebClient(IWebClient webClient) {

            switch (webClient) {

                case WebClient baseWebClient:
                    return baseWebClient;

                case WebClientAdapter webClientAdapter:
                    return webClientAdapter.InnerWebClient;

                default:
                    return null;

            }

        }

    }

}