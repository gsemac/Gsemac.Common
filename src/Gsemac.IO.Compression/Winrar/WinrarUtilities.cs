using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.Winrar {

    internal static class WinrarUtilities {

        // Public members

        public static string WinrarDirectoryPath => GetWinrarDirectoryPath();
        public static string WinrarExecutablePath => winrarExecutablePath.Value;
        public static string WinrarExecutableFilename => "Rar.exe";

        // Private members

        private static readonly Lazy<string> winrarExecutablePath = new Lazy<string>(GetWinrarExecutablePath);

        private static string GetWinrarDirectoryPath() {

            string filePath = winrarExecutablePath.Value;

            if (!string.IsNullOrWhiteSpace(filePath))
                filePath = Path.GetDirectoryName(filePath);

            return filePath;

        }
        private static string GetWinrarExecutablePath() {

            IFileSystemAssemblyResolver resolver = new FileSystemAssemblyResolver() {
                AddExtension = false,
            };

            IEnumerable<string> probingPaths = new[] {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            }.Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(path => Path.Combine(path, "WinRAR"));

            foreach (string probingPath in probingPaths.Distinct())
                resolver.ProbingPaths.Add(probingPath);

            return resolver.GetAssemblyPath(WinrarExecutableFilename);

        }

    }

}