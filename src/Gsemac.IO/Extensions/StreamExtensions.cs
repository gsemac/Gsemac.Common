using System;
using System.IO;

namespace Gsemac.IO.Extensions {

    public static class StreamExtensions {

        public static byte[] ToArray(this Stream stream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            byte[] result;
            long currentPos = -1;

            if (stream is MemoryStream memoryStream) {

                result = memoryStream.ToArray();

            }
            else {

                using (var ms = new MemoryStream()) {

                    if (stream.CanSeek) {

                        // Reset the stream's position so that we can read the entire thing.

                        currentPos = stream.Position;

                        if (currentPos != 0)
                            stream.Seek(0, SeekOrigin.Begin);

                    }

                    stream.CopyTo(ms);

                    result = ms.ToArray();

                }

            }

            // Restore the stream's position.

            if (currentPos >= 0)
                stream.Seek(currentPos, SeekOrigin.Begin);

            return result;

        }

    }

}