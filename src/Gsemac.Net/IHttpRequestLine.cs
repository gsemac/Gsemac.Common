using System;

namespace Gsemac.Net {

    public interface IHttpRequestLine :
        IHttpStartLine {

        string Method { get; }
        Uri RequestUri { get; }

    }

}