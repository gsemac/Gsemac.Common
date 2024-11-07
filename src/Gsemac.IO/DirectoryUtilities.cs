using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Gsemac.IO {

    public static class DirectoryUtilities {
        
        // Public members

        public static bool TryCreateDirectory(string directoryPath) {

            try {

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                return true;

            }
            catch {

                return false;

            }

        }
        public static bool TryCreateFileDirectory(string path) {

            if (string.IsNullOrWhiteSpace(path))
                return false;

            string parentDirectoryPath = Path.GetDirectoryName(path);

            // If the path doesn't have a parent directory, there's nothing we need to do.

            if (string.IsNullOrWhiteSpace(parentDirectoryPath))
                return true;

            return TryCreateDirectory(parentDirectoryPath);

        }
        public static bool IsDirectoryEmpty(string directoryPath) {

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
                return true;

            return !Directory.EnumerateFileSystemEntries(directoryPath).Any();

        }
        public static bool ContainsDirectories(string directoryPath) {

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
                return false;

            return Directory.EnumerateDirectories(directoryPath).Any();

        }

        public static long GetSize(string directoryPath) {

            if (!Directory.Exists(directoryPath))
                return 0;

            return new DirectoryInfo(directoryPath).EnumerateFiles("*", SearchOption.AllDirectories).Sum(fileInfo => fileInfo.Length);

        }
        public static bool TryGetSize(string directoryPath, out long directorySize) {

            directorySize = default;

            if (!Directory.Exists(directoryPath))
                return false;

            try {

                directorySize = GetSize(directoryPath);

                return true;

            }
            catch (Exception) {

                return false;

            }

        }
        public static int GetFileCount(string directoryPath) {
            return GetFileCount(directoryPath, SearchOption.TopDirectoryOnly);
        }
        public static int GetFileCount(string directoryPath, SearchOption searchOption) {

            if (!Directory.Exists(directoryPath))
                return 0;

            return Directory.EnumerateFiles(directoryPath, "*", searchOption).Count();

        }

        public static void OpenDirectory(string path, OpenDirectoryOptions options = OpenDirectoryOptions.None) {

            const string explorerExe = "explorer.exe";

            // The path provided may be a file or a directory.
            // - If the path is a directory, we'll just open the directory.
            // - If the path is a file, we'll open the directory and highlight the file.

            bool isFilePath = PathUtilities.IsFilePath(path, true);

            if (isFilePath || Directory.Exists(path))
                path = Path.GetFullPath(path);

            string explorerArguments = isFilePath ?
                $"/select, \"{path}\"" :
                $"\"{path}\"";

            if (isFilePath || options.HasFlag(OpenDirectoryOptions.NewWindow) || !Directory.Exists(path)) {

                // We choose this option if the path doesn't exist since it avoids an exception being thrown by Process.Start.
                // It will simply open a default directory.

                Process.Start(explorerExe, explorerArguments);

            }
            else {

                Process.Start(new ProcessStartInfo() {
                    FileName = path,
                    UseShellExecute = true,
                    Verb = "open"
                });

            }

        }
        public static void OpenDirectory(string path, string defaultPath, OpenDirectoryOptions options = OpenDirectoryOptions.None) {

            if (Directory.Exists(path) || File.Exists(path))
                OpenDirectory(path, options);
            else
                OpenDirectory(defaultPath, options);

        }

        public static string FindFileByName(string directoryPath, string fileName, FindFileOptions options = FindFileOptions.Default) {

            string fullPath = string.Empty;

            if (!string.IsNullOrEmpty(fileName) && Directory.Exists(directoryPath)) {

                if (options.HasFlag(FindFileOptions.IgnoreCase))
                    fileName = fileName.ToLowerInvariant();

                foreach (string filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories)) {

                    string candidateFileName = options.HasFlag(FindFileOptions.IgnoreExtension) ?
                        Path.GetFileNameWithoutExtension(filePath) :
                        Path.GetFileName(filePath);

                    if (options.HasFlag(FindFileOptions.IgnoreCase))
                        candidateFileName = candidateFileName.ToLowerInvariant();

                    if (candidateFileName.Equals(fileName)) {

                        fullPath = filePath;

                        break;

                    }

                }

            }

            return fullPath;

        }

    }

}