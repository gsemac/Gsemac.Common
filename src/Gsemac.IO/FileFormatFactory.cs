using Gsemac.Reflection;
using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO {

    public class FileFormatFactory :
        IFileFormatFactory {

        // Public members

        public static FileFormatFactory Default => new FileFormatFactory();

        public IEnumerable<IFileFormat> GetKnownFileFormats() {

            return knownFileFormats.Value;

        }

        public IFileFormat FromMimeType(IMimeType mimeType) {

            if (mimeType is null)
                throw new ArgumentNullException(nameof(mimeType));

            IFileFormat fileFormat = GetKnownFileFormatsInternal()
                .Where(format => format.MimeType.Equals(mimeType))
                .FirstOrDefault();

            return fileFormat;

        }
        public IFileFormat FromFileExtension(string fileExtension) {

            // Accepts full file paths, or plain image extensions (with or without leading period, e.g. ".jpeg" and "jpeg").

            if (fileExtension is null)
                throw new ArgumentNullException(nameof(fileExtension));

            if (string.IsNullOrWhiteSpace(fileExtension))
                throw new ArgumentException(fileExtension);

            fileExtension = PathUtilities.GetFilename(fileExtension);

            string ext = PathUtilities.GetFileExtension(fileExtension);

            if (string.IsNullOrWhiteSpace(ext))
                ext = fileExtension;
            else
                ext = PathUtilities.NormalizeFileExtension(ext);

            IFileFormat fileFormat = GetKnownFileFormatsInternal()
                .Where(format => format.Extensions.Any(formatExt => formatExt.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault();

            if (fileFormat is null)
                fileFormat = new FileFormat(ext);

            return fileFormat;

        }

        // Private members

        private static readonly Lazy<IEnumerable<IFileFormat>> knownFileFormats = new Lazy<IEnumerable<IFileFormat>>(InitializeKnownFileFormats);

        private static IEnumerable<IFileFormat> InitializeKnownFileFormats() {

            return TypeUtilities.GetTypesImplementingInterface<IFileFormat>()
                    .Where(type => type.IsDefaultConstructable())
                    .Select(type => Activator.CreateInstance(type))
                    .OfType<IFileFormat>();

        }
        private static IEnumerable<IFileFormat> GetKnownFileFormatsInternal() {

            return knownFileFormats.Value;

        }

    }

}