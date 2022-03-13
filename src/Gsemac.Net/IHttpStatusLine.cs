using System.Net;

namespace Gsemac.Net {

    public interface IHttpStatusLine :
        IHttpStartLine {

        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }

    }

}