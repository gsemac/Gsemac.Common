using Gsemac.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.IO {

    public static class PathUtilities {

        // Public members

        public const int MaxFilePathLength = 259; // 260 minus 1 for '\0'
        public const int MaxDirectoryPathLength = 247; // 248 minus 1 for '\0'
        public const int MaxPathSegmentLength = 255;
        public const string ExtendedLengthPrefix = @"\\?\";

        public static string GetRelativePath(string fullPath, string relativeToPath) {

            if (string.IsNullOrEmpty(fullPath))
                throw new ArgumentNullException(nameof(fullPath));

            if (string.IsNullOrEmpty(relativeToPath))
                return fullPath;

            string fullInputPath = System.IO.Path.GetFullPath(fullPath);
            string fullRelativeToPath = TrimDirectorySeparators(System.IO.Path.GetFullPath(relativeToPath)) + System.IO.Path.DirectorySeparatorChar;

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
        public static string GetFileName(string path) {

            // This process should work for both remote and local paths.

            string result = path is null ? string.Empty : "";

            if (!string.IsNullOrEmpty(path)) {

                try {

                    if (!Uri.TryCreate(path, UriKind.Absolute, out Uri uri))
                        uri = new Uri(new Uri("http://anything"), path);

                    result = System.IO.Path.GetFileName(uri.LocalPath);

                }
                catch (ArgumentException) {

                    // We can end up here if the path contains illegal characters (e.g. "|").
                    // Even though it shouldn't be allowed, there are URLs out there that contain them.
                    // We should still be able to handle this case.

                    Match filenameMatch = Regex.Match(path, @"(?:.*\/)?(.+?)(?:$|\?|#)");

                    if (filenameMatch.Success)
                        result = filenameMatch.Groups[1].Value;

                }
            }

            return result;

        }
        public static string GetFileNameWithoutExtension(string path) {

            string fileName = GetFileName(path);

            return StringUtilities.BeforeLast(fileName, ".");

        }
        public static string GetFileExtension(string path) {

            string filename = GetFileName(path);

            if (filename is null)
                return string.Empty;
            else if (string.IsNullOrEmpty(filename))
                return "";
            else
                return System.IO.Path.GetExtension(ReplaceInvalidPathChars(filename));

        }
        public static string SetFileExtension(string path, string extension) {

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(extension));

            if (string.IsNullOrEmpty(extension))
                throw new ArgumentNullException(nameof(extension));

            if (!IsFilePath(path, verifyFileExists: false))
                throw new ArgumentException("The given path was not a valid file path.");

            extension = extension.Trim();

            if (!extension.StartsWith("."))
                extension = "." + extension;

            string oldExtension = GetFileExtension(path);

            if (!string.IsNullOrEmpty(oldExtension)) {

                // The path already has a file extension, so replace the old extension.

                path = StringUtilities.ReplaceLast(path, oldExtension, extension);

            }
            else {

                // The path does not have a file extension, so we'll just append the new extension.

                path += extension;

            }

            return path;

        }

        public static string GetRandomTemporaryDirectoryPath() {

            return System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());

        }
        public static string GetRandomTemporaryFilePath() {

            return System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());

        }
        public static string GetUniqueTemporaryDirectoryPath() {

            lock (uniquePathMutex) {

                string result;

                // Theoretically, this could result in an infinite loop. But that won't happen... Right?

                do {

                    result = GetRandomTemporaryDirectoryPath();

                } while (System.IO.Directory.Exists(result));

                System.IO.Directory.CreateDirectory(result);

                return result;

            }

        }
        public static string GetUniqueTemporaryFilePath() {

            return System.IO.Path.GetTempFileName();

        }
        public static bool IsTemporaryFilePath(string path) {

            bool isTemporaryFilePath = false;

            try {

                path = System.IO.Path.GetFullPath(path);

                isTemporaryFilePath = path.StartsWith(System.IO.Path.GetTempPath()) && IsFilePath(path);

            }
            catch (Exception) {

                // If the path is not a valid path, then it definitely isn't a temporary file path.

            }

            return isTemporaryFilePath;

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

            return ReplaceInvalidPathChars(path, invalidCharacterReplacement, InvalidPathCharsOptions.Default | InvalidPathCharsOptions.PreserveDirectoryStructure);

        }

        public static string TrimDirectorySeparators(string path) {

            return path?.Trim(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

        }
        public static string TrimLeadingDirectorySeparators(string path) {

            return path?.TrimStart(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

        }
        public static string TrimFollowingDirectorySeparators(string path) {

            return path?.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

        }
        public static string NormalizeDirectorySeparators(string path) {

            return NormalizeDirectorySeparators(path, System.IO.Path.DirectorySeparatorChar);

        }
        public static string NormalizeDirectorySeparators(string path, char directorySeparatorChar) {

            path = string.Join(directorySeparatorChar.ToString(),
                path.Split(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar));

            return path;

        }
        public static string ReplaceInvalidPathChars(string path, InvalidPathCharsOptions options = InvalidPathCharsOptions.Default) {

            return ReplaceInvalidPathChars(path, string.Empty, options);

        }
        public static string ReplaceInvalidPathChars(string path, string replacement, InvalidPathCharsOptions options = InvalidPathCharsOptions.Default) {

            string rootPath = string.Empty;
            IEnumerable<char> invalidCharacters = Enumerable.Empty<char>();

            if (options.HasFlag(InvalidPathCharsOptions.ReplaceInvalidPathChars))
                invalidCharacters = invalidCharacters.Concat(System.IO.Path.GetInvalidPathChars());

            if (options.HasFlag(InvalidPathCharsOptions.ReplaceInvalidFileNameChars))
                invalidCharacters = invalidCharacters.Concat(System.IO.Path.GetInvalidFileNameChars());

            if (options.HasFlag(InvalidPathCharsOptions.PreserveDirectoryStructure)) {

                // The root of the path might contain characters that would be invalid file name characters (e.g. ':' in "C:\").
                // In order to preserve the root path information, we'll remove it for now and add it back later.

                rootPath = System.IO.Path.GetPathRoot(ReplaceInvalidPathChars(path, InvalidPathCharsOptions.ReplaceInvalidPathChars));

                path = path.Substring(rootPath.Length);

                invalidCharacters = invalidCharacters.Where(c => c != System.IO.Path.DirectorySeparatorChar && c != System.IO.Path.AltDirectorySeparatorChar);

            }

            path = string.Join(replacement, path.Split(invalidCharacters.ToArray()));

            if (!string.IsNullOrEmpty(rootPath))
                path = System.IO.Path.Combine(rootPath, path);

            return path;

        }

        public static bool IsFilePath(string path) {

            return IsFilePath(path, verifyFileExists: false);

        }
        public static bool IsFilePath(string path, bool verifyFileExists) {

            bool result = path.Any() &&
                path.Last() != System.IO.Path.DirectorySeparatorChar &&
                path.Last() != System.IO.Path.AltDirectorySeparatorChar;

            if (verifyFileExists && result)
                result = System.IO.File.Exists(path);

            return result;

        }
        public static bool IsLocalPath(string path) {

            return IsLocalPath(path, verifyPathExists: false);

        }
        public static bool IsLocalPath(string path, bool verifyPathExists) {

            bool isLocalPath = false;

            if (!string.IsNullOrEmpty(path)) {

                // Remove the extended length prefix if it is present. If it is present, "Uri.TryCreate" will fail.
                // Since we're not creating any files, we don't need to worry about it.

                if (path.StartsWith(ExtendedLengthPrefix))
                    path = path.Substring(ExtendedLengthPrefix.Length);

                if (Uri.TryCreate(path, UriKind.Absolute, out Uri testUri)) {

                    // "IsFile" returns true for both local file and directory paths.
                    // "IsUnc" will return true for paths beginning with "\\" or "//" (the latter case gets turned into "file://"). 

                    isLocalPath = testUri.IsFile && !testUri.IsUnc;

                    if (isLocalPath && verifyPathExists)
                        isLocalPath = PathExists(testUri.LocalPath);

                }
                else if (new char[] { System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar }.All(c => c != path.First()) && Uri.TryCreate(path, UriKind.Relative, out _)) {

                    // Check the full path for this relative path.
                    // We initially avoid cases where the path begins with a directory separator as those should be considered rooted.

                    path = System.IO.Path.GetFullPath(path);

                    isLocalPath = IsLocalPath(path, verifyPathExists: verifyPathExists);

                }

            }

            return isLocalPath;

        }
        public static bool IsPathRooted(string path) {

            // "System.IO.Path.IsPathRooted" throws an exception for paths longer than the maximum path length, as well as for malformed paths (e.g. paths containing invalid characters).

            string directorySeparatorsStr = System.IO.Path.DirectorySeparatorChar.ToString() + System.IO.Path.AltDirectorySeparatorChar.ToString();
            string pattern = @"^(?:[" + Regex.Escape(directorySeparatorsStr) + @"]|[a-z]+\:\/\/|[a-z]\:)";

            return Regex.IsMatch(path, pattern, RegexOptions.IgnoreCase);

        }
        public static bool IsPathTooLong(string path) {

            // For the purpose of checking the length, replace all illegal characters in the path.
            // This will ensure Path methods don't throw.

            path = ReplaceInvalidPathChars(path, " ", InvalidPathCharsOptions.PreserveDirectoryStructure);

            path = System.IO.Path.GetFullPath(path);

            // Check the length of the entire path.

            if (IsFilePath(path, false)) {

                if (System.IO.Path.GetDirectoryName(path).Length > MaxDirectoryPathLength || path.Length > MaxFilePathLength)
                    return true;

            }
            else {

                path = TrimDirectorySeparators(path);

                if (path.Length > MaxDirectoryPathLength)
                    return true;

            }

            // Check the length of each segment.

            if (GetPathSegments(path).Any(segment => TrimDirectorySeparators(segment).Length > MaxPathSegmentLength))
                return true;

            return false;

        }
        public static bool PathExists(string path) {

            return System.IO.Directory.Exists(path) || System.IO.File.Exists(path);

        }
        public static bool PathContainsSegment(string path, string pathSegment) {

            pathSegment = TrimDirectorySeparators(pathSegment);

            return GetPathSegments(path)
                .Select(segment => TrimDirectorySeparators(segment))
                .Any(segment => segment.Equals(pathSegment, StringComparison.OrdinalIgnoreCase));

        }
        public static bool AreEqual(string path1, string path2, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase) {

            return GetPathSegments(path1).SequenceEqual(GetPathSegments(path2), StringUtilities.GetStringComparer(stringComparison));

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

        // Private members

        private static readonly object uniquePathMutex = new object();

    }

}