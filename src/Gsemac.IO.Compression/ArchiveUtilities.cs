using Gsemac.IO.Compression.Extensions;
using System.IO;

namespace Gsemac.IO.Compression {

    public static class ArchiveUtilities {

        // Public members

        public static IArchive Open(string filePath, IFileFormat archiveFormat, IArchiveOptions archiveOptions = null) {

            return ArchiveFactory.Default.Open(filePath, archiveFormat, archiveOptions);

        }
        public static IArchive Open(string filePath, IArchiveOptions archiveOptions = null) {

            return ArchiveFactory.Default.Open(filePath, archiveOptions);

        }
        public static IArchive Open(string filePath, FileAccess fileAccess) {

            return ArchiveFactory.Default.Open(filePath, fileAccess);

        }
        public static IArchive OpenRead(string filePath) {

            return ArchiveFactory.Default.OpenRead(filePath);

        }
        public static IArchive OpenWrite(string filePath) {

            return ArchiveFactory.Default.OpenWrite(filePath);

        }

        public static void Extract(string filePath, bool extractToNewFolder = true) {

            string outputPath = Path.GetDirectoryName(filePath);

            if (extractToNewFolder)
                outputPath = Path.Combine(outputPath, PathUtilities.GetFileNameWithoutExtension(filePath));

            Extract(filePath, outputPath);

        }
        public static void Extract(string filePath, string directoryPath) {

            using (IArchive archive = Open(filePath, FileAccess.Read))
                archive.ExtractAllEntries(directoryPath);

        }

        public static void ExtractFile(string archiveFilePath, string filePathInArchive, string outputFilePath) {

            string directoryPath = Path.GetDirectoryName(outputFilePath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using (IArchive archive = OpenRead(archiveFilePath))
            using (FileStream outputStream = File.OpenWrite(outputFilePath))
                archive.ExtractEntry(archive.GetEntry(filePathInArchive), outputStream);

        }

    }

}