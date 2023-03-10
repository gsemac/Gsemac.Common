using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Gsemac.Core {

    public static class ProcessUtilities {

        // Public members

        public static IEnumerable<Process> GetProcessesByFileName(string fileName) {

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

            string processExecutablePath = process.MainModule.FileName;

            return !string.IsNullOrWhiteSpace(processExecutablePath) && processExecutablePath.Equals(filePath);

        }

    }

}