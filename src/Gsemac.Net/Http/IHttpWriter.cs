using System.IO;
using Gsemac.Net.Http.Headers;

namespace Gsemac.Net.Http
{

    public interface IHttpWriter {

        void WriteHeader(IHttpHeader header);
        void WriteBody(Stream body);

    }

}