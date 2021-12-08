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
                .Where(format => format.MimeType.Equals(mimeType))
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
                ext = PathUtilities.GetFilename(filePath);
            else
                ext = PathUtilities.NormalizeFileExtension(ext);

            IFileFormat fileFormat = GetKnownFileFormats()
                .Where(format => format.Extensions.Any(formatExt => formatExt.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault();

            return fileFormat;

        }
        public IFileFormat FromStream(Stream stream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            using (MemoryStream streamBytes = new MemoryStream()) {

                IFileFormat fileFormat = GetKnownFileFormats()
                    .Where(format => format.Signatures.Any())
                    .OrderByDescending(format => format.Signatures.Max(sig => sig.Length))
                    .Where(format => {

                        return format.Signatures.Any(sig => {

                            if (sig.Length > streamBytes.Length) {

                                int count = (int)(sig.Length - streamBytes.Length);
                                byte[] buffer = new byte[sig.Length - streamBytes.Length];

                                stream.Read(buffer, 0, count);

                                streamBytes.Seek(0, SeekOrigin.End);

                                streamBytes.Write(buffer, 0, count);

                            }

                            streamBytes.Seek(0, SeekOrigin.Begin);

                            return sig.IsMatch(streamBytes);

                        });

                    })
                    .FirstOrDefault();

                return fileFormat;

            }

        }

        // Protected members

        protected abstract IEnumerable<IFileFormat> GetKnownFileFormats();

    }

}