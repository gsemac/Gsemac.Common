using System.Net;

namespace Gsemac.Net.Extensions {

    public static class WebRequestExtensions {

        // Public members

        public static IHttpWebRequest AsHttpWebRequest(this WebRequest request) {

            switch (request) {

                case HttpWebRequest httpWebRequest:
                    return new HttpWebRequestAdapter(httpWebRequest);

                case IHttpWebRequest iHttpWebRequest:
                    return iHttpWebRequest;

                default:
                    return null;

            }

        }

        // Internal members

        internal static WebRequest GetInnermostWebRequest(this WebRequest webRequest) {

            WebRequest result = webRequest;
            bool exitLoop = false;

            while (!exitLoop && result is object) {

                switch (result) {

                    case HttpWebRequestAdapter httpWebRequestAdapter:
                        result = httpWebRequestAdapter.InnerWebRequest;
                        break;

                    case HttpWebRequestDecoratorBase httpWebRequestDecoratorBase:
                        result = httpWebRequestDecoratorBase.InnerWebRequest;
                        break;

                    default:
                        exitLoop = true;
                        break;

                }

            }

            return result;

        }

    }

}