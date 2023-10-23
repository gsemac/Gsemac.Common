using System.IO;

namespace Gsemac.Net.Dns {

    internal interface IDnsMessageSerializer {

        void Serialize(Stream stream, IDnsMessage message);
        IDnsMessage Deserialize(Stream stream);

    }

}