namespace Gsemac.Net.Http {

    public interface IHttpResponseWriter :
        IHttpWriter {

        void WriteStatusLine(IHttpStatusLine statusLine);

    }

}