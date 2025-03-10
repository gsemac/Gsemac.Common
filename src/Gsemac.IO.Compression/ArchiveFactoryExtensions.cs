using System;
using System.IO;

namespace Gsemac.IO.Compression {

    public static class ArchiveFactoryExtensions {

        // Public members

        public static IArchive Open(this IArchiveFactory archiveFactory, Stream stream) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return archiveFactory.Open(stream, GetArchiveOptionsFromStream(stream));

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, Stream stream, IArchiveOptions options) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (options is null)
                options = GetArchiveOptionsFromStream(stream);

            stream = FileFormatFactory.Default.FromStream(stream, out IFileFormat archiveFormat);

            return archiveFactory.Open(stream, archiveFormat, archiveOptions: options);

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, Stream stream, IFileFormat format) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (format is null)
                stream = FileFormatFactory.Default.FromStream(stream, out format);

            return archiveFactory.Open(stream, format, ArchiveOptions.Default);

        }

        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            return archiveFactory.Open(filePath, options: null);

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath, FileAccess fileAccess) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            return archiveFactory.Open(filePath, new ArchiveOptions() {
                FileAccess = fileAccess,
            });

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath, IArchiveOptions options) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            return archiveFactory.Open(filePath, null, options);

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath, IFileFormat format, IArchiveOptions options) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            if (format is null)
                format = FileFormatFactory.Default.FromFileExtension(filePath);

            FileAccess fileAccess = options?.FileAccess ?? FileAccess.ReadWrite;
            FileMode fileMode = fileAccess == FileAccess.Read ? FileMode.Open : FileMode.OpenOrCreate;
            FileStream fileStream = new FileStream(filePath, fileMode, fileAccess);

            try {

                return archiveFactory.Open(fileStream, format, options);

            }
            catch {

                fileStream.Dispose();

                throw;

            }

        }

        public static IArchive OpenRead(this IArchiveFactory archiveFactory, string filePath) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            return archiveFactory.Open(filePath, FileAccess.Read);

        }
        public static IArchive OpenWrite(this IArchiveFactory archiveFactory, string filePath) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            return archiveFactory.Open(filePath, FileAccess.Write);

        }

        // Private members

        private static IArchiveOptions GetArchiveOptionsFromStream(Stream stream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return new ArchiveOptions() {
                FileAccess = StreamUtilities.GetFileAccessFromStream(stream),
            };

        }

    }

}