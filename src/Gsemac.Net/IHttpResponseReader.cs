using System;
using System.Collections.Generic;

namespace Gsemac.Net {

    public interface IHttpResponseReader :
        IDisposable {

        IHttpStatusLine ReadStatusLine();
        bool TryReadStatusLine(out IHttpStatusLine result);
        IEnumerable<IHttpHeader> ReadHeaders();
        string ReadBody();

    }

}