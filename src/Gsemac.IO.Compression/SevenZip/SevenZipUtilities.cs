using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.SevenZip {

    internal static class SevenZipUtilities {

        // Public members

        public static string SevenZipDirectoryPath => GetSevenZipDirectoryPath();
        public static string SevenZipExecutablePath => sevenZipExecutablePath.Value;
        public static string SevenZipExecutableFilename => "7z.exe";

        // Private members

        private static readonly Lazy<string> sevenZipExecutablePath = new Lazy<string>(GetSevenZipExecutablePath);

        private static string GetSevenZipDirectoryPath() {

            string filePath = sevenZipExecutablePath.Value;

            if (!string.IsNullOrWhiteSpace(filePath))
                filePath = Path.GetDirectoryName(filePath);

            return filePath;

        }
        private static string GetSevenZipExecutablePath() {

            IFileSystemAssemblyResolver resolver = new FileSystemAssemblyResolver() {
                AddExtension = false,
            };

            IEnumerable<string> probingPaths = new[] {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            }.Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(path => Path.Combine(path, "7-Zip"));

            foreach (string probingPath in probingPaths.Distinct())
                resolver.ProbingPaths.Add(probingPath);

            return resolver.GetAssemblyPath(SevenZipExecutableFilename);

        }

    }

}