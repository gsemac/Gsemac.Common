using System;

namespace Gsemac.Net.Http {

    public interface IHttpStartLine {

        Version ProtocolVersion { get; }

    }

}