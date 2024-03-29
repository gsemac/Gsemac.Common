﻿using System;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    public static class ArchiveExtensions {

        public static bool AddFile(this IArchive archive, string filePath, bool overwrite = true) {

            string entryName = PathUtilities.GetFileName(filePath);

            return archive.AddFile(filePath, entryName, overwrite);

        }
        public static bool AddFile(this IArchive archive, string filePath, string entryName, bool overwrite = true) {

            if (!overwrite && archive.ContainsEntry(entryName))
                return false;

            Stream stream = File.OpenRead(filePath);

            try {

                archive.AddEntry(stream, entryName);

            }
            catch (Exception) {

                // If we fail to add the entry, make sure we close the stream we opened.

                stream.Close();

                throw;

            }

            return true;

        }

        public static void AddAllFiles(this IArchive archive, string directoryPath, bool overwrite = true) {

            foreach (string filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
                archive.AddFile(filePath, PathUtilities.GetRelativePath(filePath, directoryPath), overwrite);

        }

        public static IArchiveEntry AddEntry(this IArchive archive, Stream stream, string entryName) {

            return archive.AddEntry(stream, entryName, ArchiveEntryOptions.Default);

        }

        public static bool ContainsEntry(this IArchive archive, string entryName) {

            return archive.GetEntry(entryName) is object;

        }
        public static bool ContainsEntry(this IArchive archive, IArchiveEntry entry) {

            return archive.GetEntries().Any(e => e.Equals(entry));

        }

        public static bool DeleteEntry(this IArchive archive, string entryName) {

            IArchiveEntry entry = archive.GetEntry(entryName);

            if (entry is object)
                archive.DeleteEntry(entry);

            return entry is object;

        }

        public static void ExtractEntry(this IArchive archive, IArchiveEntry entry, string filePath) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

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
        public static void ExtractEntry(this IArchive archive, string entryName, string filePath) {

            IArchiveEntry entry = archive.GetEntry(entryName);

            archive.ExtractEntry(entry, filePath);

        }
        public static void ExtractEntry(this IArchive archive, string entryName, Stream outputStream) {

            IArchiveEntry entry = archive.GetEntry(entryName);

            archive.ExtractEntry(entry, outputStream);

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
        public static void ExtractAllEntries(this IArchive archive) {

            archive.ExtractAllEntries(Directory.GetCurrentDirectory());

        }

    }

}