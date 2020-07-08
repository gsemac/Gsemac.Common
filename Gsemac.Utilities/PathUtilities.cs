using System;
using System.Diagnostics;
using System.Linq;

namespace Gsemac.Utilities {

    [Flags]
    public enum OpenPathOptions {
        None,
        Default = None,
        /// <summary>
        /// Open the path a new window even if it is already open.
        /// </summary>
        NewWindow
    }

    public static class PathUtilities {

        // Public members

        public static string GetRelativePath(string fullPath, string relativePath) {

            if (string.IsNullOrEmpty(fullPath))
                throw new ArgumentNullException(nameof(fullPath));

            if (string.IsNullOrEmpty(relativePath))
                return fullPath;

            string fullInputPath = System.IO.Path.GetFullPath(fullPath);
            string fullRelativeToPath = TrimDirectorySeparators(System.IO.Path.GetFullPath(relativePath)) + System.IO.Path.DirectorySeparatorChar;

            int index = fullInputPath.IndexOf(fullRelativeToPath);

            if (index >= 0)
                return fullInputPath.Substring(index + fullRelativeToPath.Length, fullInputPath.Length - (index + fullRelativeToPath.Length));
            else
                return fullPath;

        }
        public static string GetPathRelativeToRoot(string path) {

            string relativePath = path;

            if (!string.IsNullOrWhiteSpace(relativePath)) {

                relativePath = TrimDirectorySeparators(path.Substring(System.IO.Path.GetPathRoot(path).Length));

            }

            return relativePath;

        }
        public static string AnonymizePath(string path) {

            // Remove the current user's username from the path (if the path is inside the user directory).

            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (!string.IsNullOrWhiteSpace(userDirectory) && !string.IsNullOrWhiteSpace(path)) {

                // Make the paths relative to their root so that the path can be anonymized regardless of the drive it's on.

                userDirectory = GetPathRelativeToRoot(userDirectory);
                path = GetPathRelativeToRoot(path);

                if (path.StartsWith(userDirectory)) {

                    // If the path is directly inside of %USERPROFILE%, make the path relative to that variable.

                    path = System.IO.Path.Combine("%USERPROFILE%", GetRelativePath(path, userDirectory));

                }

            }

            return path;

        }
        public static string TrimDirectorySeparators(string path) {

            return path?.Trim(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

        }
        public static bool IsFilePath(string path) {

            return path.Any() &&
                path.Last() != System.IO.Path.DirectorySeparatorChar &&
                path.Last() != System.IO.Path.AltDirectorySeparatorChar &&
                System.IO.File.Exists(path);

        }

        public static void OpenPath(string path, OpenPathOptions options = OpenPathOptions.None) {

            const string explorerExe = "explorer.exe";

            // The path provided may be a file or a directory.
            // - If the path is a directory, we'll just open the directory.
            // - If the path is a file, we'll open the directory and highlight the file.

            string explorerArguments = $"\"{path}\"";
            bool isFilePath = IsFilePath(path);

            if (isFilePath)
                explorerArguments = $"/select, \"{path}\"";

            if (isFilePath || System.IO.Directory.Exists(path))
                path = System.IO.Path.GetFullPath(path);

            if (isFilePath || options.HasFlag(OpenPathOptions.NewWindow) || !System.IO.Directory.Exists(path)) {

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
        public static void OpenPathOrDefault(string path, string defaultPath, OpenPathOptions options = OpenPathOptions.None) {

            if (System.IO.Directory.Exists(path) || System.IO.File.Exists(path))
                OpenPath(path, options);
            else
                OpenPath(defaultPath, options);

        }

    }

}