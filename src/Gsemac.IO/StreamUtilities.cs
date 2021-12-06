using System.IO;
using System.Text;

namespace Gsemac.IO {

    public static class StreamUtilities {

        // Public members

        public const int DefaultBufferSize = 81920; // https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.copyto?view=net-6.0

        public static string StreamToString(Stream stream) {

            return StreamToString(stream, Encoding.UTF8);

        }
        public static string StreamToString(Stream stream, Encoding encoding) {

            using (StreamReader sr = new StreamReader(stream, encoding))
                return sr.ReadToEnd();

        }

    }

}