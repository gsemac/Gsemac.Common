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
        public static IArchive Open(this IArchiveFactory archiveFactory, Stream stream, IArchiveOptions archiveOptions) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveOptions is null)
                archiveOptions = GetArchiveOptionsFromStream(stream);

            stream = FileFormatFactory.Default.FromStream(stream, out IFileFormat archiveFormat);

            return archiveFactory.Open(stream, archiveFormat, archiveOptions: archiveOptions);

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, Stream stream, IFileFormat archiveFormat) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            return archiveFactory.Open(stream, archiveFormat, ArchiveOptions.Default);

        }

        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            return archiveFactory.Open(filePath, archiveOptions: null);

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath, FileAccess fileAccess) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            return archiveFactory.Open(filePath, new ArchiveOptions() {
                FileAccess = fileAccess,
            });

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath, IArchiveOptions archiveOptions) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            return archiveFactory.Open(filePath, null, archiveOptions);

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (archiveFactory is null)
                throw new ArgumentNullException(nameof(archiveFactory));

            if (archiveFormat is null)
                archiveFormat = FileFormatFactory.Default.FromFileExtension(filePath);

            FileAccess fileAccess = archiveOptions?.FileAccess ?? FileAccess.ReadWrite;
            FileMode fileMode = fileAccess == FileAccess.Read ? FileMode.Open : FileMode.OpenOrCreate;
            FileStream fileStream = new FileStream(filePath, fileMode, fileAccess);

            try {

                return archiveFactory.Open(fileStream, archiveFormat, archiveOptions);

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