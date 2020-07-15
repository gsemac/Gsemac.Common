using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Gsemac.Utilities {

    public static class PathUtilities {

        // Public members

        public const int MaxFilePathLength = 259; // 260 minus 1 for '\0'
        public const int MaxDirectoryPathLength = 247; // 248 minus 1 for '\0'
        public const int MaxPathSegmentLength = 255;

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
        public static string GetRelativePathToRoot(string path) {

            string relativePath = path;

            if (!string.IsNullOrWhiteSpace(relativePath)) {

                relativePath = TrimDirectorySeparators(path.Substring(System.IO.Path.GetPathRoot(path).Length));

            }

            return relativePath;

        }
        public static IEnumerable<string> GetPathSegments(string path) {

            // Get the root of the path first, as it might contain multiple directory separators.

            string pathRoot = System.IO.Path.GetPathRoot(path);

            if (!string.IsNullOrEmpty(pathRoot)) {

                // On Windows, GetPathRoot can return just a drive letter (e.g. "C:"). To signify that this is a path, append a directory separator.
                // The separator is not always appended, because it might be a single separator (e.g. "/") on Unix systems.

                if (pathRoot.Last() != System.IO.Path.DirectorySeparatorChar && pathRoot.Last() != System.IO.Path.AltDirectorySeparatorChar) {

                    pathRoot += System.IO.Path.DirectorySeparatorChar.ToString();

                }

                path = path.Substring(pathRoot.Length);

                yield return pathRoot;

            }

            // Return the remaining segments.

            IEnumerable<string> remainingSegments = StringUtilities.SplitAfter(path, System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

            foreach (string segment in remainingSegments)
                yield return segment;

        }

        public static string AnonymizePath(string path) {

            // Remove the current user's username from the path (if the path is inside the user directory).

            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (!string.IsNullOrWhiteSpace(userDirectory) && !string.IsNullOrWhiteSpace(path)) {

                // Make the paths relative to their root so that the path can be anonymized regardless of the drive it's on.

                userDirectory = GetRelativePathToRoot(userDirectory);
                path = GetRelativePathToRoot(path);

                if (path.StartsWith(userDirectory)) {

                    // If the path is directly inside of %USERPROFILE%, make the path relative to that variable.

                    path = System.IO.Path.Combine("%USERPROFILE%", GetRelativePath(path, userDirectory));

                }

            }

            return path;

        }
        public static string SanitizePath(string path, string invalidCharacterReplacement = "_") {

            return ReplaceInvalidPathChars(path, invalidCharacterReplacement, InvalidPathCharacterOptions.Default | InvalidPathCharacterOptions.AllowPathSeparators);

        }

        public static string TrimDirectorySeparators(string path) {

            return path?.Trim(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

        }
        public static string ReplaceInvalidPathChars(string path, InvalidPathCharacterOptions options = InvalidPathCharacterOptions.Default) {

            return ReplaceInvalidPathChars(path, string.Empty, options);

        }
        public static string ReplaceInvalidPathChars(string path, string replacement, InvalidPathCharacterOptions options = InvalidPathCharacterOptions.Default) {

            IEnumerable<char> invalidCharacters = Enumerable.Empty<char>();

            if (options.HasFlag(InvalidPathCharacterOptions.IncludeInvalidPathCharacters))
                invalidCharacters = invalidCharacters.Concat(System.IO.Path.GetInvalidPathChars());

            if (options.HasFlag(InvalidPathCharacterOptions.IncludeInvalidFileNameCharacters))
                invalidCharacters = invalidCharacters.Concat(System.IO.Path.GetInvalidFileNameChars());

            if (options.HasFlag(InvalidPathCharacterOptions.AllowPathSeparators))
                invalidCharacters = invalidCharacters.Where(c => c != System.IO.Path.DirectorySeparatorChar && c != System.IO.Path.AltDirectorySeparatorChar);

            path = string.Join(replacement, path.Split(invalidCharacters.ToArray()));

            return path;

        }

        public static bool IsFilePath(string path, bool verifyFileExists = true) {

            bool result = path.Any() &&
                path.Last() != System.IO.Path.DirectorySeparatorChar &&
                path.Last() != System.IO.Path.AltDirectorySeparatorChar;

            if (verifyFileExists && result)
                result = System.IO.File.Exists(path);

            return result;

        }
        public static bool IsPathTooLong(string path) {

            // For the purpose of checking the length, replace all illegal characters in the path.
            // This will ensure Path methods don't throw.

            path = ReplaceInvalidPathChars(path, " ");

            path = System.IO.Path.GetFullPath(path);

            // Check the length of the entire path.

            if (IsFilePath(path, false)) {

                if (path.Length > MaxFilePathLength)
                    return false;

            }
            else {

                if (path.Length > MaxDirectoryPathLength)
                    return false;

            }

            // Check the length of each segment.

            if (GetPathSegments(path).Any(segment => segment.Length > MaxPathSegmentLength))
                return false;

            return true;

        }

        public static void OpenPath(string path, OpenPathOptions options = OpenPathOptions.None) {

            const string explorerExe = "explorer.exe";

            // The path provided may be a file or a directory.
            // - If the path is a directory, we'll just open the directory.
            // - If the path is a file, we'll open the directory and highlight the file.

            bool isFilePath = IsFilePath(path, true);

            if (isFilePath || System.IO.Directory.Exists(path))
                path = System.IO.Path.GetFullPath(path);

            string explorerArguments = isFilePath ?
                $"/select, \"{path}\"" :
                $"\"{path}\"";

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