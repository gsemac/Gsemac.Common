using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Curl {

    public class BinCurlHttpWebResponse :
        CurlHttpWebResponseBase {

        // Public members

        /// <summary>
        /// Initializes a new instance of the <see cref="BinCurlHttpWebResponse"/> class.
        /// </summary>
        public BinCurlHttpWebResponse(IHttpWebRequest parentRequest, BinCurlProcessStream responseStream) :
            base(parentRequest, responseStream) {

            // If we got an error status code (or nothing at all), throw an exception.

            if (StatusCode == 0) {

                // We didn't read a status code from the stream at all.
                throw new WebException("Did not receive a valid HTTP response.", null, WebExceptionStatus.ServerProtocolViolation, this);

            }
            else if (!((int)StatusCode).ToString().StartsWith("2")) {

                // We got a response, but didn't get a success code.
                throw new WebException(string.Format("Server responded with error.", (int)StatusCode), null, WebExceptionStatus.ProtocolError, this);

            }

        }

    }

}