using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO {

    public abstract class FileFormatFactoryBase :
        IFileFormatFactory {

        // Public members

        public IFileFormat FromMimeType(IMimeType mimeType) {

            if (mimeType is null)
                throw new ArgumentNullException(nameof(mimeType));

            IFileFormat fileFormat = GetKnownFileFormats()
                .Where(format => format.MimeTypes.Any(m => m.Equals(mimeType)))
                .FirstOrDefault();

            return fileFormat;

        }
        public IFileFormat FromFileExtension(string filePath) {

            // Accepts full file paths, or plain image extensions (with or without leading period, e.g. ".jpeg" and "jpeg").

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(filePath);

            string ext = PathUtilities.GetFileExtension(filePath);

            if (string.IsNullOrWhiteSpace(ext))
                ext = PathUtilities.GetFileName(filePath);
            else
                ext = PathUtilities.NormalizeFileExtension(ext);

            // Find formats with a matching extension, prioritizing more restrictive formats (fewer overall file extensions).
            // For example, if a file has the ".webm" extension, it's more likely to be a WebM than an generic MKV.

            IFileFormat fileFormat = GetKnownFileFormats()
                .Where(format => format.Extensions.Any(formatExt => formatExt.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(format => format.Extensions.Count())
                .FirstOrDefault();

            return fileFormat;

        }
        public IFileFormat FromStream(Stream stream, int bufferSize) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            using (MemoryStream streamBytes = new MemoryStream(bufferSize)) {

                streamBytes.SetLength(stream.Read(streamBytes.GetBuffer(), 0, bufferSize));

                // Find formats with a matching signature, prioritizing less restrictive formats (more overall file extensions).
                // For example, if a file has an MKV signature, we can't know for certain if it's a WebM or not without analyzing the contents.

                IFileFormat fileFormat = GetKnownFileFormats()
                    .Where(format => format.Signatures.Any())
                    .OrderByDescending(format => format.Signatures.Max(sig => sig.Length))
                    .Where(format => FormatIsMatch(format, stream))
                    .OrderByDescending(format => format.Extensions.Count())
                    .FirstOrDefault();

                return fileFormat;

            }

        }

        // Protected members

        protected abstract IEnumerable<IFileFormat> GetKnownFileFormats();

        // Private members

        private static bool FormatIsMatch(IFileFormat fileFormat, Stream stream) {

            if (fileFormat is null)
                throw new ArgumentNullException(nameof(fileFormat));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return fileFormat.Signatures.Any(sig => {

                stream.Seek(0, SeekOrigin.Begin);

                return sig.IsMatch(stream);

            });

        }

    }

}