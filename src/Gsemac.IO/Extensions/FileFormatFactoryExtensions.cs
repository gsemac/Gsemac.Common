using System;
using System.IO;

namespace Gsemac.IO.Extensions {

    public static class FileFormatFactoryExtensions {

        // Public members

        public static IFileFormat FromFile(this IFileFormatFactory fileFormatFactory, string filePath) {

            using (Stream stream = File.OpenRead(filePath))
                return fileFormatFactory.FromStream(stream) ?? fileFormatFactory.FromFileExtension(filePath);

        }
        public static IFileFormat FromMimeType(this IFileFormatFactory fileFormatFactory, string mimeType) {

            return fileFormatFactory.FromMimeType(new MimeType(mimeType));

        }

        public static IFileFormat FromStream(this IFileFormatFactory fileFormatFactory, Stream stream) {

            return fileFormatFactory.FromStream(stream, FileFormatFactory.DefaultReadBufferSize);

        }
        public static Stream FromStream(this IFileFormatFactory fileFormatFactory, Stream stream, out IFileFormat fileFormat) {

            return FromStream(fileFormatFactory, stream, bufferSize: FileFormatFactory.DefaultReadBufferSize, out fileFormat);

        }
        public static Stream FromStream(this IFileFormatFactory fileFormatFactory, Stream stream, int bufferSize, out IFileFormat fileFormat) {

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

                fileSignatureBuffer.Dispose();

                throw;

            }

        }

    }

}