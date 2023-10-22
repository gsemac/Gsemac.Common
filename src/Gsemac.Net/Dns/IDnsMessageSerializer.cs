using System.IO;

namespace Gsemac.Net.Dns {

    internal interface IDnsMessageSerializer {

        void Serialize(IDnsMessage message, Stream stream);
        IDnsMessage Deserialize(Stream stream);

    }

}