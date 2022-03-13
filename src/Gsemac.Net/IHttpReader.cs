using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Net {

    public interface IHttpReader :
    IDisposable {

        IHttpStartLine StartLine { get; }
        IEnumerable<IHttpHeader> Headers { get; }

        bool ReadStartLine(out IHttpStartLine startLine);
        bool ReadHeader(out IHttpHeader header);

        Stream GetBodyStream();

    }

}