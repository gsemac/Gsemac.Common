using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Gsemac.Core {

    public static class ProcessUtilities {

        // Public members

        public static IEnumerable<Process> GetProcessesByFileName(string fileName) {

            // TODO: It might be better to check the path associated with the process and compare the file name instead of the process name.
            // This approach tends to work in practice, but I don't know if the process name always matches the filename.

            if (string.IsNullOrWhiteSpace(fileName))
                return Enumerable.Empty<Process>();

            string processName = Path.GetFileNameWithoutExtension(fileName);

            if (string.IsNullOrEmpty(processName))
                return Enumerable.Empty<Process>();

            return Process.GetProcessesByName(processName);

        }
        public static IEnumerable<Process> GetProcessesByFilePath(string filePath) {

            return GetProcessesByFileName(filePath)
                .Where(process => ProcessHasFilePath(process, filePath));

        }

        // Private members

        private static bool ProcessHasFilePath(Process process, string filePath) {

            if (process is null)
                throw new ArgumentNullException(nameof(process));

            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            string processPath = process.MainModule.FileName;

            if (string.IsNullOrWhiteSpace(processPath))
                return false;

            // Normalize the directory separators before doing the path comparison.

            return NormalizeProcessPath(filePath)
                .Equals(NormalizeProcessPath(processPath));


        }
        private static string NormalizeProcessPath(string processPath) {

            if (string.IsNullOrWhiteSpace(processPath))
                return processPath;

            return Path.GetFullPath(processPath)
                .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

        }

    }

}