using System;

namespace Gsemac.Net.Http {

    public interface IHttpRequestLine :
        IHttpStartLine {

        string Method { get; }
        Uri RequestUri { get; }

    }

}