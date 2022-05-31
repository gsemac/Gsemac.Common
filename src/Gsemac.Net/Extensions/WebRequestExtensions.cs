using Gsemac.Net.Http;
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

    }

}