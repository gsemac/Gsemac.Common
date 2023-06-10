using System;
using System.Net;
using System.Text;

namespace Gsemac.Net.Http {

    public static class HttpWebResponseUtilities {

        // Public members

        public static bool TryGetEncoding(HttpWebResponse response, out Encoding encoding) {

            return TryGetEncoding(new HttpWebResponseAdapter(response), out encoding);

        }
        public static bool TryGetEncoding(IHttpWebResponse response, out Encoding encoding) {

            if (response is null)
                throw new ArgumentNullException(nameof(response));

            encoding = null;

            string responseCharacterSet = response.CharacterSet;

            if (!string.IsNullOrWhiteSpace(responseCharacterSet)) {

                try {

                    encoding = Encoding.GetEncoding(responseCharacterSet);

                    return true;

                }
                catch (Exception) {

                    // It's possible that the server specified an invalid character set.

                }

            }

            return false;

        }

    }

}