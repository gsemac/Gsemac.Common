using System;
using System.IO;

namespace Gsemac.IO {

    public static class StreamUtilities {

        // Public members

        public const int DefaultCopyBufferSize = 81920; // https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.copyto?view=net-6.0

        public static FileAccess GetFileAccessFromStream(Stream stream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            FileAccess fileAccess = 0;

            if (stream.CanRead)
                fileAccess |= FileAccess.Read;

            if (stream.CanWrite)
                fileAccess |= FileAccess.Write;

            return fileAccess;

        }

    }

}