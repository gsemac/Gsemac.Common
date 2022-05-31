using System.Net;

namespace Gsemac.Net.Http {

    public interface IHttpStatusLine :
        IHttpStartLine {

        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }

    }

}