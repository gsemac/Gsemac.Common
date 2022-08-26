using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Net.Http {

    public interface IHttpReader :
    IDisposable {

        IHttpStartLine StartLine { get; }
        IEnumerable<IHttpHeader> Headers { get; }

        bool TryReadStartLine(out IHttpStartLine startLine);
        bool TryReadHeader(out IHttpHeader header);

        Stream GetBodyStream();

    }

}