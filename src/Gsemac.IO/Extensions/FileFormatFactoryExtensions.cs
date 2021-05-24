using System;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Extensions {

    public static class FileFormatFactoryExtensions {

        public static IFileFormat FromFile(this IFileFormatFactory fileFormatFactory, string filePath) {

            using (Stream stream = File.OpenRead(filePath))
                return fileFormatFactory.FromStream(stream) ?? fileFormatFactory.FromFileExtension(filePath);

        }
        public static IFileFormat FromMimeType(this IFileFormatFactory fileFormatFactory, string mimeType) {

            return fileFormatFactory.FromMimeType(new MimeType(mimeType));

        }
        public static Stream FromStream(this IFileFormatFactory fileFormatFactory, Stream stream, out IFileFormat fileFormat) {

            // Read the file signature from the stream into a buffer, which is then concatenated with the original stream.
            // This allows us to read the file signature from streams that don't support seeking.

            int maxSignatureLength = fileFormatFactory.GetKnownFileFormats()
                .Where(format => format.Signatures is object && format.Signatures.Any())
                .SelectMany(format => format.Signatures.Select(sig => sig.Length))
                .OrderByDescending(length => length)
                .FirstOrDefault();

            MemoryStream fileSignatureBuffer = new MemoryStream(new byte[maxSignatureLength], 0, maxSignatureLength, writable: true, publiclyVisible: true);

            try {

                int bytesRead = stream.Read(fileSignatureBuffer.GetBuffer(), 0, maxSignatureLength);

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