using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.Extensions {

    public static class ArchiveExtensions {

        public static void AddFile(this IArchive archive, string filePath) {

            archive.AddFile(filePath, filePath);

        }
        public static void AddFile(this IArchive archive, string filePath, string entryName) {

            archive.AddEntry(File.OpenRead(filePath), entryName, leaveOpen: false);

        }
        public static void AddAllFiles(this IArchive archive, string directoryPath) {

            foreach (string filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
                archive.AddFile(filePath, PathUtilities.GetRelativePath(filePath, directoryPath));

        }
        public static bool DeleteEntry(this IArchive archive, string entryName) {

            IArchiveEntry entry = archive.GetEntry(entryName);

            if (!(entryName is null))
                archive.DeleteEntry(entry);

            return !(entryName is null);

        }
        public static void ExtractEntry(this IArchive archive, IArchiveEntry entry, string filePath) {

            // Some implementations do not create the file path's directory automatically.

            string directoryPath = Path.GetDirectoryName(filePath);
            bool createdDirectory = false;

            if (!string.IsNullOrWhiteSpace(directoryPath) && !Directory.Exists(directoryPath)) {

                Directory.CreateDirectory(directoryPath);

                createdDirectory = true;

            }

            try {

                using (FileStream fs = File.OpenWrite(filePath))
                    archive.ExtractEntry(entry, fs);

            }
            finally {

                // Delete the output directory if it is empty (we weren't able to extract the file).

                if (createdDirectory && !Directory.EnumerateFileSystemEntries(directoryPath).Any())
                    Directory.Delete(directoryPath);

            }

        }
        public static void ExtractAllEntries(this IArchive archive, string directoryPath) {

            bool createdDirectory = false;

            if (!string.IsNullOrWhiteSpace(directoryPath) && !Directory.Exists(directoryPath)) {

                Directory.CreateDirectory(directoryPath);

                createdDirectory = true;

            }

            try {

                foreach (IArchiveEntry entry in archive.GetEntries())
                    archive.ExtractEntry(entry, Path.Combine(directoryPath, PathUtilities.NormalizeDirectorySeparators(entry.Name)));

            }
            finally {

                // Delete the output directory if it is empty (we weren't able to extract the files).

                if (createdDirectory && !Directory.EnumerateFileSystemEntries(directoryPath).Any())
                    Directory.Delete(directoryPath);

            }

        }

    }

}