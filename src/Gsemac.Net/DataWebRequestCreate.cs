using System;
using System.Net;

namespace Gsemac.Net {

    public sealed class DataWebRequestCreate :
        IWebRequestCreate {

        public WebRequest Create(Uri uri) {

            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            return new DataWebRequest(uri);

        }

    }

}