namespace Gsemac.Net.Http {

    public interface IHttpResponseReader :
        IHttpReader {

        new IHttpStatusLine StartLine { get; }

        bool ReadStartLine(out IHttpStatusLine startLine);

    }

}