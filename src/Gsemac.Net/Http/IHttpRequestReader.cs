namespace Gsemac.Net.Http {

    public interface IHttpRequestReader :
        IHttpReader {

        new IHttpRequestLine StartLine { get; }

        bool ReadStartLine(out IHttpRequestLine startLine);

    }

}