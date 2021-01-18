using System;
using System.Diagnostics;

namespace Gsemac.IO {

    public static class DirectoryUtilities {

        public static string FindFileByFilename(string directoryPath, string filename, FindFileOptions options = FindFileOptions.Default) {

            string fullPath = string.Empty;

            if (!string.IsNullOrEmpty(filename) && System.IO.Directory.Exists(directoryPath)) {

                if (options.HasFlag(FindFileOptions.IgnoreCase))
                    filename = filename.ToLowerInvariant();

                foreach (string filePath in System.IO.Directory.EnumerateFiles(directoryPath, "*", System.IO.SearchOption.AllDirectories)) {

                    string candidateFileName = options.HasFlag(FindFileOptions.IgnoreExtension) ?
                        System.IO.Path.GetFileNameWithoutExtension(filePath) :
                        System.IO.Path.GetFileName(filePath);

                    if (options.HasFlag(FindFileOptions.IgnoreCase))
                        candidateFileName = candidateFileName.ToLowerInvariant();

                    if (candidateFileName.Equals(filename)) {

                        fullPath = filePath;

                        break;

                    }

                }

            }

            return fullPath;

        }

        public static bool TryCreateDirectory(string directoryPath) {

            try {

                if (!System.IO.Directory.Exists(directoryPath))
                    System.IO.Directory.CreateDirectory(directoryPath);

                return true;

            }
            catch (Exception) {

                return false;

            }

        }

        public static void OpenDirectory(string path, OpenDirectoryOptions options = OpenDirectoryOptions.None) {

            const string explorerExe = "explorer.exe";

            // The path provided may be a file or a directory.
            // - If the path is a directory, we'll just open the directory.
            // - If the path is a file, we'll open the directory and highlight the file.

            bool isFilePath = PathUtilities.IsFilePath(path, true);

            if (isFilePath || System.IO.Directory.Exists(path))
                path = System.IO.Path.GetFullPath(path);

            string explorerArguments = isFilePath ?
                $"/select, \"{path}\"" :
                $"\"{path}\"";

            if (isFilePath || options.HasFlag(OpenDirectoryOptions.NewWindow) || !System.IO.Directory.Exists(path)) {

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

            if (System.IO.Directory.Exists(path) || System.IO.File.Exists(path))
                OpenDirectory(path, options);
            else
                OpenDirectory(defaultPath, options);

        }

    }

}