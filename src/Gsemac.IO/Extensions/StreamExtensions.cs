using System;
using System.IO;

namespace Gsemac.IO.Extensions {

    public static class StreamExtensions {

        // Public members

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

        public static void CopyTo(this Stream source, Stream destination, int bufferSize, int count) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (destination is null)
                throw new ArgumentNullException(nameof(destination));

            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize), Core.Properties.ExceptionMessages.PositiveNumberRequired);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), Core.Properties.ExceptionMessages.NonNegativeNumberRequired);

            int bytesToRead = count;

            byte[] buffer = new byte[bufferSize];

            while (bytesToRead > 0) {

                int bytesRead = source.Read(buffer, 0, buffer.Length);

                // It's tempting to stop reading if bytesRead < bufferSize, because this implies there aren't enough bytes to fill the buffer and we have therefore reached the end.
                // This is not strictly true, as noted in the documentation for Stream.Read: "An implementation is free to return fewer bytes than requested even if the end of the stream has not been reached."
                // https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.read?view=net-6.0

                if (bytesRead > 0)
                    destination.Write(buffer, 0, bytesRead);
                else
                    break;

            }

        }

    }

}