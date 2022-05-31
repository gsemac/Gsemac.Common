using System.IO;

namespace Gsemac.Net.Http {

    public interface IHttpWriter {

        void WriteHeader(IHttpHeader header);
        void WriteBody(Stream body);

    }

}