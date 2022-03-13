namespace Gsemac.Net {

    public interface IHttpResponseReader :
        IHttpReader {

        new IHttpStatusLine StartLine { get; }

        bool ReadStartLine(out IHttpStatusLine startLine);

    }

}