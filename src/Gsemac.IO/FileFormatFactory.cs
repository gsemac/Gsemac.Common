﻿using Gsemac.IO.FileFormats;
using Gsemac.Reflection;
using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
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

            IFileFormat fileFormat = GetKnownFileFormatsInternal()
                .Where(format => format.Extensions.Any(formatExt => formatExt.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault();

            if (fileFormat is null)
                fileFormat = new FileFormat(ext);

            return fileFormat;

        }
        public IFileFormat FromStream(Stream stream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            using (MemoryStream streamBytes = new MemoryStream()) {

                IFileFormat fileFormat = GetKnownFileFormatsInternal()
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

        // Private members

        private static readonly Lazy<IEnumerable<IFileFormat>> knownFileFormats = new Lazy<IEnumerable<IFileFormat>>(InitializeKnownFileFormats);

        private static IEnumerable<IFileFormat> InitializeKnownFileFormats() {

            return TypeUtilities.GetTypesImplementingInterface<IFileFormat>()
                .Where(type => type != typeof(AnyFileFormat))
                .Where(type => type.IsDefaultConstructable())
                .Select(type => Activator.CreateInstance(type))
                .OfType<IFileFormat>();

        }
        private static IEnumerable<IFileFormat> GetKnownFileFormatsInternal() {

            return knownFileFormats.Value;

        }

    }

}