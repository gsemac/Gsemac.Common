using Gsemac.Net.Http;
using System.Net;

namespace Gsemac.Net.Extensions {

    public static class WebResponseExtensions {

        public static IHttpWebResponse AsHttpWebRequest(this WebResponse response) {

            switch (response) {

                case HttpWebResponse httpWebResponse:
                    return new HttpWebResponseAdapter(httpWebResponse);

                case IHttpWebResponse iHttpWebResponse:
                    return iHttpWebResponse;

                default:
                    return null;

            }

        }

    }

}