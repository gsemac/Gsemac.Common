using Gsemac.Reflection;
using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO {

    public class FileFormat :
        FileFormatBase {

        // Public members

        public override IEnumerable<string> Extensions { get; } = Enumerable.Empty<string>();
        public override IEnumerable<IFileSignature> Signatures { get; } = Enumerable.Empty<IFileSignature>();
        public override string MimeType { get; } = string.Empty;

        public static IFileFormat FromFileExtension(string filePath) {

            // Accepts full file paths, or plain image extensions (with or without leading period, e.g. ".jpeg" and "jpeg").

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(filePath);

            string filename = PathUtilities.GetFilename(filePath);
            string ext = PathUtilities.GetFileExtension(filename);

            if (string.IsNullOrWhiteSpace(ext))
                ext = filename;
            else
                ext = PathUtilities.NormalizeFileExtension(ext);

            IFileFormat fileFormat = GetKnownFileFormats()
                .Where(format => format.Extensions.Any(formatExt => formatExt.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault();

            if (fileFormat is null)
                fileFormat = new FileFormat(ext);

            return fileFormat;

        }
        public static IFileFormat FromMimeType(string mimeType) {

            if (string.IsNullOrEmpty(mimeType))
                throw new ArgumentNullException(nameof(mimeType));

            IFileFormat fileFormat = GetKnownFileFormats()
                .Where(format => format.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            // We were unable to find a file format with a matching MIME type.

            return null;

        }

        // Private members

        private static readonly Lazy<IEnumerable<IFileFormat>> knownFileFormats = new Lazy<IEnumerable<IFileFormat>>(InitializeKnownFileFormats);

        private FileFormat(string fileExtension) {

            if (fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
                MimeType = "text/plain";
            else
                MimeType = "application/octet-stream";

            Extensions = new string[] {
                fileExtension
            };

        }

        private static IEnumerable<IFileFormat> InitializeKnownFileFormats() {

            return TypeUtilities.GetTypesImplementingInterface<IFileFormat>()
                    .Where(type => type.IsDefaultConstructable())
                    .Select(type => Activator.CreateInstance(type))
                    .OfType<IFileFormat>();

        }
        private static IEnumerable<IFileFormat> GetKnownFileFormats() {

            return knownFileFormats.Value;

        }

    }

}