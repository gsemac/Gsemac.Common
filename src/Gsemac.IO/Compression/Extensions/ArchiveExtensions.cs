using System.IO;

namespace Gsemac.IO.Compression.Extensions {

    public static class ArchiveExtensions {

        public static void AddEntry(this IArchive archive, string filePath) {

            archive.AddEntry(filePath, filePath);

        }
        public static void AddEntry(this IArchive archive, string filePath, string entryName) {

            archive.AddEntry(File.OpenRead(filePath), entryName, leaveOpen: false);

        }
        public static void AddAllEntries(this IArchive archive, string directoryPath) {

            foreach (string filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
                archive.AddEntry(filePath, PathUtilities.GetRelativePath(filePath, directoryPath));

        }
        public static bool DeleteEntry(this IArchive archive, string entryName) {

            IArchiveEntry entry = archive.GetEntry(entryName);

            if (!(entryName is null))
                archive.DeleteEntry(entry);

            return !(entryName is null);

        }
        public static void ExtractEntry(this IArchive archive, IArchiveEntry entry, string filePath) {

            using (FileStream fs = File.OpenWrite(filePath))
                archive.ExtractEntry(entry, fs);

        }
        public static void ExtractAllEntries(this IArchive archive, string directoryPath) {

            foreach (IArchiveEntry entry in archive.GetEntries())
                archive.ExtractEntry(entry, Path.Combine(directoryPath, entry.Path));

        }

    }

}