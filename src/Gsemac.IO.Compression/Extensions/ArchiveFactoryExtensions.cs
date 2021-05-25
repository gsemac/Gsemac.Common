using System.IO;

namespace Gsemac.IO.Compression.Extensions {

    public static class ArchiveFactoryExtensions {

        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath) {

            return Open(archiveFactory, filePath, null);

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath, FileAccess fileAccess) {

            return Open(archiveFactory, filePath, new ArchiveOptions() {
                FileAccess = fileAccess,
            });

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath, IArchiveOptions archiveOptions) {

            return Open(archiveFactory, filePath, null, archiveOptions);

        }
        public static IArchive Open(this IArchiveFactory archiveFactory, string filePath, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

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
        public static IArchive Open(this IArchiveFactory archiveFactory, Stream stream, IArchiveOptions archiveOptions) {

            return archiveFactory.Open(stream, archiveFormat: null, archiveOptions: archiveOptions);

        }

        public static IArchive OpenRead(this IArchiveFactory archiveFactory, string filePath) {

            return Open(archiveFactory, filePath, FileAccess.Read);

        }
        public static IArchive OpenWrite(this IArchiveFactory archiveFactory, string filePath) {

            return Open(archiveFactory, filePath, FileAccess.Write);

        }

    }

}