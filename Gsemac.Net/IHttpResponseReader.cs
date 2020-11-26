using System;
using System.Collections.Generic;

namespace Gsemac.Net {

    public interface IHttpResponseReader :
        IDisposable {

        IHttpStatusLine ReadStatusLine();
        IEnumerable<IHttpHeader> ReadHeaders();
        string ReadBody();

    }

}