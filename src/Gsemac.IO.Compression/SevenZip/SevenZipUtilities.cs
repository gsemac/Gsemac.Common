using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.SevenZip {

    internal static class SevenZipUtilities {

        // Public members

        public static string SevenZipExecutablePath => sevenZipExecutablePath.Value;

        // Private members

        private static readonly Lazy<string> sevenZipExecutablePath = new Lazy<string>(GetSevenZipExecutablePath);

        private static string GetSevenZipExecutablePath() {

            IFileSystemAssemblyResolver resolver = new FileSystemAssemblyResolver() {
                AddExtension = false,
            };

            IEnumerable<string> probingPaths = new[] {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            }.Select(path => Path.Combine(path, "7-Zip"));

            foreach (string probingPath in probingPaths.Distinct())
                resolver.ProbingPaths.Add(probingPath);

            return resolver.GetAssemblyPath("7z.exe");

        }

    }

}