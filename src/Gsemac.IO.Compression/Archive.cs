using System;
using System.IO;

namespace Gsemac.IO.Compression {

    public static class Archive {

        // Public members

        public static IArchive Open(string filePath, IFileFormat format) {
            return Open(filePath, format, ArchiveOptions.Default);
        }
        public static IArchive Open(string filePath, IFileFormat format, IArchiveOptions options) {
            return ArchiveFactory.Default.Open(filePath, format, options);
        }
        public static IArchive Open(string filePath) {
            return Open(filePath, ArchiveOptions.Default);
        }
        public static IArchive Open(string filePath, IArchiveOptions options) {
            return ArchiveFactory.Default.Open(filePath, options);
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

        public static void Extract(string filePath) {

            Extract(filePath, new ArchiveExtractionOptions() {
                ExtractToNewFolder = true,
            });

        }
        public static void Extract(string filePath, string directoryPath) {

            Extract(filePath, new ArchiveExtractionOptions() {
                OutputDirectoryPath = directoryPath,
                ExtractToNewFolder = false,
            });

        }
        public static void Extract(string filePath, bool extractToNewFolder) {

            Extract(filePath, new ArchiveExtractionOptions() {
                ExtractToNewFolder = extractToNewFolder,
            });

        }
        public static void Extract(string filePath, IArchiveExtractionOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            string outputPath = string.IsNullOrWhiteSpace(options.OutputDirectoryPath) ?
                Path.GetDirectoryName(filePath) :
                options.OutputDirectoryPath;

            if (options.ExtractToNewFolder)
                outputPath = Path.Combine(outputPath, PathUtilities.GetFileNameWithoutExtension(filePath));

            IArchiveOptions archiveOptions = new ArchiveOptions() {
                Password = options.Password,
                FileAccess = FileAccess.Read,
            };

            using (IArchive archive = Open(filePath, archiveOptions))
                archive.ExtractAllEntries(outputPath);

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