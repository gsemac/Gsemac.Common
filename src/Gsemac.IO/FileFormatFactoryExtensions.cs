using System;
using System.IO;

namespace Gsemac.IO {

    public static class FileFormatFactoryExtensions {

        // Public members

        public static IFileFormat FromFile(this IFileFormatFactory fileFormatFactory, string filePath) {

            if (fileFormatFactory is null)
                throw new ArgumentNullException(nameof(fileFormatFactory));

            if (File.Exists(filePath)) {

                using (Stream stream = File.OpenRead(filePath))
                    return fileFormatFactory.FromStream(stream) ?? fileFormatFactory.FromFileExtension(filePath);

            }
            else {

                return fileFormatFactory.FromFileExtension(filePath);

            }

        }
        public static IFileFormat FromMimeType(this IFileFormatFactory fileFormatFactory, string mimeType) {

            if (fileFormatFactory is null)
                throw new ArgumentNullException(nameof(fileFormatFactory));

            return fileFormatFactory.FromMimeType(new MimeType(mimeType));

        }

        public static IFileFormat FromStream(this IFileFormatFactory fileFormatFactory, Stream stream) {

            if (fileFormatFactory is null)
                throw new ArgumentNullException(nameof(fileFormatFactory));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return fileFormatFactory.FromStream(stream, FileFormatFactory.DefaultReadBufferSize);

        }
        public static Stream FromStream(this IFileFormatFactory fileFormatFactory, Stream stream, out IFileFormat fileFormat) {

            if (fileFormatFactory is null)
                throw new ArgumentNullException(nameof(fileFormatFactory));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return fileFormatFactory.FromStream(stream, bufferSize: FileFormatFactory.DefaultReadBufferSize, out fileFormat);

        }
        public static Stream FromStream(this IFileFormatFactory fileFormatFactory, Stream stream, int bufferSize, out IFileFormat fileFormat) {

            if (fileFormatFactory is null)
                throw new ArgumentNullException(nameof(fileFormatFactory));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (stream.CanSeek) {

                // If the stream is seekable, we can read directly from the stream and then seek back.

                long initialPosition = stream.Position;

                fileFormat = fileFormatFactory.FromStream(stream);

                stream.Seek(initialPosition, SeekOrigin.Begin);

                return stream;

            }
            else {

                // Read the file signature from the stream into a buffer, which is then concatenated with the original stream.
                // This allows us to read the file signature from streams that don't support seeking.

                MemoryStream fileSignatureBuffer = new MemoryStream(new byte[bufferSize], 0, count: bufferSize, writable: true, publiclyVisible: true);

                try {

                    int bytesRead = stream.Read(fileSignatureBuffer.GetBuffer(), 0, FileFormatFactory.DefaultReadBufferSize);

                    fileSignatureBuffer.SetLength(bytesRead);

                    fileFormat = fileFormatFactory.FromStream(fileSignatureBuffer);

                    fileSignatureBuffer.Seek(0, SeekOrigin.Begin);

                    return new ConcatStream(fileSignatureBuffer, stream);

                }
                catch (Exception) {

                    // The buffer is ONLY disposed if we fail to return a ConcatStream instance.

                    fileSignatureBuffer.Dispose();

                    throw;

                }

            }

        }

    }

}