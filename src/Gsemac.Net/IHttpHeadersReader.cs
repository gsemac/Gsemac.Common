using System;

namespace Gsemac.Net {

    public interface IHttpHeadersReader :
        IDisposable {

        bool ReadStatusLine(out IHttpStatusLine statusLine);
        bool ReadNextHeader(out IHttpHeader header);

    }

}