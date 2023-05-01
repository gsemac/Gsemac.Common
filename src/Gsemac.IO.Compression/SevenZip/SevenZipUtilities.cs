using System;
using System.IO;

namespace Gsemac.IO.Compression.SevenZip {

    internal static class SevenZipUtilities {

        // Public members

        public static string SevenZipDirectoryPath => GetSevenZipDirectoryPath();
        public static string SevenZipExecutablePath => sevenZipExecutablePath.Value;
        public static string SevenZipExecutableFileName => "7z.exe";

        // Private members

        private static readonly Lazy<string> sevenZipExecutablePath = new Lazy<string>(GetSevenZipExecutablePath);

        private static string GetSevenZipDirectoryPath() {

            string filePath = sevenZipExecutablePath.Value;

            if (!string.IsNullOrWhiteSpace(filePath))
                filePath = Path.GetDirectoryName(filePath);

            return filePath;

        }
        private static string GetSevenZipExecutablePath() {

            return ArchiveFactoryUtilities.GetProgramExecutablePath("7-Zip", SevenZipExecutableFileName);

        }

    }

}