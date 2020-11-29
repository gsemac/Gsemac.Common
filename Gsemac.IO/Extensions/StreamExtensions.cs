using System.IO;

namespace Gsemac.IO.Extensions {

    public static class StreamExtensions {

        public static byte[] ToArray(this Stream stream) {

            if (stream is MemoryStream memoryStream) {

                return memoryStream.ToArray();

            }
            else {

                using (var ms = new MemoryStream()) {

                    stream.CopyTo(ms);

                    return ms.ToArray();

                }

            }

        }

    }

}