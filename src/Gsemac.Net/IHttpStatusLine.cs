using System;
using System.Net;

namespace Gsemac.Net {

    public interface IHttpStatusLine {

        Version ProtocolVersion { get; }
        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }

    }

}