using Gsemac.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.IO {

    public delegate string CharReplacementEvaluatorDelegate(char inputChar);

    public static class PathUtilities {

        // Public members

        public const int MaxFilePathLength = 259; // 260 minus 1 for '\0'
        public const int MaxDirectoryPathLength = 247; // 248 minus 1 for '\0'
        public const int MaxPathSegmentLength = 255;
        public const string ExtendedLengthPrefix = @"\\?\";

        public static string GetPath(string path) {

            string relativePath = path;

            if (!string.IsNullOrWhiteSpace(relativePath))
                relativePath = path.Substring(GetRootPath(path).Length);

            if (!IsUrl(path))
                relativePath = TrimLeftDirectorySeparators(relativePath);

            return relativePath;

        }
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
        public static string GetRootPath(string path) {

            return GetRootPath(path, new PathInfo());

        }
        public static string GetRootPath(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            string rootPath = string.Empty;
            string scheme = GetScheme(path);

            if (!string.IsNullOrWhiteSpace(scheme)) {

                Match rootMatch = Regex.Match(path.Substring(scheme.Length), @":[\/\\]{2}[^\/\\]+(?:[\/\\]|$)");

                if (rootMatch.Success)
                    rootPath = path.Substring(0, scheme.Length + rootMatch.Length);

            }
            else if (!string.IsNullOrEmpty(path)) {

                // If the path starts with a single slash, consider that to be the root.
                // Don't consider the path rooted if it's a URL that starts with a forward slash (it's relative).

                if (!(IsUrl(path, pathInfo) && (path.StartsWith("/") || path.StartsWith("\\")))) {

                    Match rootMatch = Regex.Match(path, @"^([\/\\]{1})[^\/\\]");

                    if (rootMatch.Success)
                        rootPath = rootMatch.Groups[1].Value;
                    else
                        rootPath = System.IO.Path.GetPathRoot(path);

                }

            }

            if (!string.IsNullOrEmpty(rootPath) && !rootPath.All(c => c.Equals(System.IO.Path.DirectorySeparatorChar) || c.Equals(System.IO.Path.AltDirectorySeparatorChar)))
                rootPath = TrimRightDirectorySeparators(rootPath);

            return rootPath;

        }
        public static string GetParentPath(string path) {

            return GetParentPath(path, new PathInfo());

        }
        public static string GetParentPath(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            string rootPath = GetRootPath(path, pathInfo);
            int separatorIndex = TrimRightDirectorySeparators(path).LastIndexOfAny(new[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar });

            if (separatorIndex < rootPath.Length)
                return string.Empty;

            return path.Substring(0, separatorIndex);

        }
        public static IEnumerable<string> GetPathSegments(string path) {

            // Get the root of the path first, as it might contain multiple directory separators.

            string pathRoot = GetRootPath(path);

            if (!string.IsNullOrEmpty(pathRoot)) {

                path = path.Substring(pathRoot.Length);

                // On Windows, GetPathRoot can return just a drive letter (e.g. "C:"). To signify that this is a path, append a directory separator.
                // The separator is not always appended, because it might be a single separator (e.g. "/") on Unix systems.

                if (StartsWithDirectorySeparatorChar(path) && !EndsWithDirectorySeparatorChar(pathRoot)) {

                    pathRoot += path.First();
                    path = path.Substring(1);

                }

                yield return pathRoot;

            }

            // Return the remaining segments.

            IEnumerable<string> remainingSegments = StringUtilities.SplitAfter(path, System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar)
                .Where(segment => !string.IsNullOrEmpty(segment));

            foreach (string segment in remainingSegments)
                yield return segment;

        }
        public static int GetPathDepth(string path, PathDepthOptions options = PathDepthOptions.Default) {

            if (string.IsNullOrWhiteSpace(path))
                return 0;

            path = NormalizeDirectorySeparators(TrimLeftDirectorySeparators(GetPath(path)));

            if (options.HasFlag(PathDepthOptions.IgnoreTrailingDirectorySeparators))
                path = TrimRightDirectorySeparators(path);

            if (string.IsNullOrWhiteSpace(path))
                return 0;

            return path.Split(new[] { System.IO.Path.DirectorySeparatorChar }).Count();

        }
        public static string GetScheme(string path) {

            Match match = Regex.Match(path, @"^([\w][\w+-.]+):");

            if (match.Success)
                return match.Groups[1].Value;

            return string.Empty;

        }

        public static string GetFilename(string path) {

            return GetFilename(path, new PathInfo());

        }
        public static string GetFilename(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            // This process should work for both remote and local paths.
            // The user is optionally able to specify explicitly whether the path is a URL or a local path.

            if ((pathInfo.IsUrl.HasValue && pathInfo.IsUrl.Value) || (!pathInfo.IsUrl.HasValue && IsUrl(path)))
                return GetFilenameFromUrl(path);

            // If the path cannot be determined to be a URL, treat it like a local path.
            // Invalid path characters are be allowed (e.g. "|"), and content after the hash character ("#") should be included in the filename.
            // While this signifies the start of a URI fragment for URLs, it is a valid path character on Windows and most Linux flavors.

            return GetFilenameWithRegex(path, pathInfo.IsUrl ?? false);

        }
        public static string GetFilenameWithoutExtension(string path) {

            string fileName = GetFilename(path);

            return StringUtilities.BeforeLast(fileName, ".");

        }
        public static string GetFileExtension(string path) {

            string filename = GetFilename(path);

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
        public static string NormalizeFileExtension(string extension) {

            if (extension is null)
                throw new ArgumentNullException(nameof(extension));

            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException(Properties.ExceptionMessages.InvalidFileExtension, nameof(extension));

            extension = extension.ToLowerInvariant()
                .Trim('.')
                .Trim();

            extension = "." + extension;

            return extension;

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
        public static bool IsUrl(string path) {

            if (string.IsNullOrWhiteSpace(path))
                return false;

            return !string.IsNullOrWhiteSpace(GetScheme(path));

        }
        public static bool IsUrl(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            if (pathInfo.IsUrl.HasValue)
                return pathInfo.IsUrl.Value;

            return IsUrl(path);

        }

        public static string AnonymizePath(string path) {

            // Remove the current user's username from the path (if the path is inside the user directory).

            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (!string.IsNullOrWhiteSpace(userDirectory) && !string.IsNullOrWhiteSpace(path)) {

                // Make the paths relative to their root so that the path can be anonymized regardless of the drive it's on.

                userDirectory = GetPath(userDirectory);
                path = GetPath(path);

                if (path.StartsWith(userDirectory)) {

                    // If the path is directly inside of %USERPROFILE%, make the path relative to that variable.

                    path = System.IO.Path.Combine("%USERPROFILE%", GetRelativePath(path, userDirectory));

                }

            }

            return path;

        }
        public static string SanitizePath(string path, SanitizePathOptions options = SanitizePathOptions.Default) {

            if (options.HasFlag(SanitizePathOptions.UseEquivalentValidPathChars)) {

                bool inQuotes = false;

                return SanitizePath(path, c => GetEquivalentValidPathChar(c, ref inQuotes), options);

            }
            else {

                return SanitizePath(path, string.Empty, options);

            }

        }
        public static string SanitizePath(string path, string replacement, SanitizePathOptions options = SanitizePathOptions.Default) {

            return SanitizePath(path, c => replacement, options);

        }
        public static string SanitizePath(string path, CharReplacementEvaluatorDelegate replacementEvaluator, SanitizePathOptions options = SanitizePathOptions.Default) {

            return ReplaceInvalidPathChars(path, replacementEvaluator, options);

        }

        public static string TrimDirectorySeparators(string path) {

            return path?.Trim(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

        }
        public static string TrimLeftDirectorySeparators(string path) {

            return path?.TrimStart(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

        }
        public static string TrimRightDirectorySeparators(string path) {

            return path?.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);

        }
        public static string NormalizeDirectorySeparators(string path) {

            return NormalizeDirectorySeparators(path, new PathInfo());

        }
        public static string NormalizeDirectorySeparators(string path, char directorySeparatorChar) {


            path = string.Join(directorySeparatorChar.ToString(),
                path.Split(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar));

            return path;

        }
        public static string NormalizeDirectorySeparators(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            if (string.IsNullOrEmpty(path))
                return path;

            char directorySeparatorChar = System.IO.Path.DirectorySeparatorChar;

            if (IsUrl(path, pathInfo))
                directorySeparatorChar = '/';

            return NormalizeDirectorySeparators(path, directorySeparatorChar);

        }

        public static bool IsFilePath(string path, bool verifyFileExists = false) {

            bool result = path.Any() &&
                path.Last() != System.IO.Path.DirectorySeparatorChar &&
                path.Last() != System.IO.Path.AltDirectorySeparatorChar;

            if (verifyFileExists && result)
                result = System.IO.File.Exists(path);

            return result;

        }
        public static bool IsLocalPath(string path, bool verifyPathExists = false) {

            bool isLocalPath = false;

            if (!string.IsNullOrEmpty(path)) {

                // Remove the extended length prefix if it is present. If it is present, "Uri.TryCreate" will fail.
                // Since we're not creating any files, we don't need to worry about it.

                if (path.StartsWith(ExtendedLengthPrefix))
                    path = path.Substring(ExtendedLengthPrefix.Length);

                if (Uri.TryCreate(path, UriKind.Absolute, out Uri testUri)) {

                    // "IsFile" returns true for both local file and directory paths.
                    // "IsUnc" will return true for paths beginning with "\\" or "//" (the latter case gets turned into "file://"). 

                    isLocalPath = testUri.IsFile && !testUri.IsUnc &&
                        (!verifyPathExists || PathExists(testUri.LocalPath));

                }
                else if (new char[] { System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar }.Any(c => c == path.First())) {

                    // Rooted paths that don't specify a scheme will be considered local.

                    isLocalPath = !verifyPathExists || PathExists(testUri.LocalPath);

                }
                else if (Uri.TryCreate(path, UriKind.Relative, out _)) {

                    // Check the full path for this relative path.

                    path = System.IO.Path.GetFullPath(path);

                    isLocalPath = IsLocalPath(path, verifyPathExists: verifyPathExists);

                }

            }

            return isLocalPath;

        }
        public static bool IsPathRooted(string path) {

            return IsPathRooted(path, new PathInfo());

        }
        public static bool IsPathRooted(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            // "System.IO.Path.IsPathRooted" throws an exception for paths longer than the maximum path length, as well as for malformed paths (e.g. paths containing invalid characters).

            // URLs starting with path separators are not rooted, but relative to the root.

            if (IsUrl(path, pathInfo) && (path.StartsWith("/") || path.StartsWith("\\")))
                return false;

            string directorySeparatorsStr = System.IO.Path.DirectorySeparatorChar.ToString() + System.IO.Path.AltDirectorySeparatorChar.ToString();
            string pattern = @"^(?:[" + Regex.Escape(directorySeparatorsStr) + @"]|[a-z]+\:\/\/|[a-z]\:)";

            return Regex.IsMatch(path, pattern, RegexOptions.IgnoreCase);

        }
        public static bool IsPathTooLong(string path) {

            // For the purpose of checking the length, replace all illegal characters in the path.
            // This will ensure Path methods don't throw.

            path = ReplaceInvalidPathChars(path, " ", SanitizePathOptions.PreserveDirectoryStructure);

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

            // Consider the paths equal if they are both null.

            if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2))
                return true;

            // If only one of the paths is null, the paths are not equal.

            if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2))
                return false;

            // Directory separators are ignored when comparing paths.

            path1 = NormalizeDirectorySeparators(path1);
            path2 = NormalizeDirectorySeparators(path2);

            return path1.Equals(path2, stringComparison);

        }

        // Private members

        private static readonly object uniquePathMutex = new object();

        private static bool EndsWithDirectorySeparatorChar(string path) {

            return !string.IsNullOrEmpty(path) &&
                (path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) ||
                path.EndsWith(System.IO.Path.AltDirectorySeparatorChar.ToString()));

        }
        private static string ReplaceInvalidPathChars(string path, SanitizePathOptions options = SanitizePathOptions.Default) {

            return ReplaceInvalidPathChars(path, string.Empty, options);

        }
        private static string ReplaceInvalidPathChars(string path, string replacement, SanitizePathOptions options = SanitizePathOptions.Default) {

            return ReplaceInvalidPathChars(path, c => replacement, options);

        }
        private static string ReplaceInvalidPathChars(string path, CharReplacementEvaluatorDelegate replacementEvaluator, SanitizePathOptions options = SanitizePathOptions.Default) {

            string rootOrScheme = string.Empty;
            IEnumerable<char> invalidCharacters = Enumerable.Empty<char>();

            // To match the behavior of popular web browsers, trim excess forward slashes after the scheme when using HTTP/HTTPS.
            // https://github.com/whatwg/url/issues/118
            // This is only done when the "PreserveDirectoryStructure" flag is toggled, because otherwise the slashes are replaced anyway (as may be desired).

            if (options.HasFlag(SanitizePathOptions.PreserveDirectoryStructure))
                StripRepeatedForwardSlashesAfterScheme(path);

            // Normalize directory separators.

            if (options.HasFlag(SanitizePathOptions.NormalizeDirectorySeparators))
                path = NormalizeDirectorySeparators(path);

            if (options.HasFlag(SanitizePathOptions.StripInvalidPathChars))
                invalidCharacters = invalidCharacters.Concat(System.IO.Path.GetInvalidPathChars());

            if (options.HasFlag(SanitizePathOptions.StripInvalidFilenameChars))
                invalidCharacters = invalidCharacters.Concat(System.IO.Path.GetInvalidFileNameChars());

            if (options.HasFlag(SanitizePathOptions.PreserveDirectoryStructure)) {

                // The root of the path might contain characters that would be invalid file name characters (e.g. ':' in "C:\").
                // In order to preserve the root path information, we'll remove it for now and add it back later.

                rootOrScheme = GetRootOrScheme(path);

                if (!string.IsNullOrEmpty(rootOrScheme))
                    path = path.Substring(rootOrScheme.Length);

                invalidCharacters = invalidCharacters.Where(c => c != System.IO.Path.DirectorySeparatorChar && c != System.IO.Path.AltDirectorySeparatorChar);

            }

            // Strip repeated directory separators.

            if (options.HasFlag(SanitizePathOptions.StripRepeatedDirectorySeparators))
                StripRepeatedDirectorySeparators(path);

            HashSet<char> invalidCharacterLookup = new HashSet<char>(invalidCharacters);
            StringBuilder pathBuilder = new StringBuilder();

            foreach (char c in path.ToCharArray()) {

                if (invalidCharacterLookup.Contains(c))
                    pathBuilder.Append(replacementEvaluator(c));
                else
                    pathBuilder.Append(c);

            }

            path = pathBuilder.ToString();

            if (!string.IsNullOrEmpty(rootOrScheme))
                path = rootOrScheme + TrimLeftDirectorySeparators(path);

            return path;

        }
        private static string GetEquivalentValidPathChar(char inputChar, ref bool inQuotes) {

            switch (inputChar) {

                case '\0':
                case '\u0001':
                case '\u0002':
                case '\u0003':
                case '\u0004':
                case '\u0005':
                case '\u0006':
                case '\a':
                case '\b':
                case '\n':
                case '\v':
                case '\f':
                case '\r':
                case '\u000e':
                case '\u000f':
                case '\u0010':
                case '\u0011':
                case '\u0012':
                case '\u0013':
                case '\u0014':
                case '\u0015':
                case '\u0016':
                case '\u0017':
                case '\u0018':
                case '\u0019':
                case '\u001a':
                case '\u001b':
                case '\u001c':
                case '\u001d':
                case '\u001e':
                case '\u001f':
                    return string.Empty;

                case '\t':
                    return " ";

                case '"':

                    inQuotes = !inQuotes;

                    return !inQuotes ? "”" : "“";

                case '*':
                    return "＊";

                case '/':
                    return "⁄";

                case ':':
                    return "∶";

                case '<':
                    return "＜";

                case '>':
                    return "＞";

                case '?':
                    return "？";

                case '\\':
                    return "＼";

                case '|':
                    return "｜";

                default:
                    return inputChar.ToString();

            }

        }
        private static string GetFilenameFromUrl(string path) {

            if (path is null)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(path))
                return "";

            try {

                if (!Uri.TryCreate(path, UriKind.Absolute, out Uri uri))
                    uri = new Uri(new Uri("http://anything"), path);

                return System.IO.Path.GetFileName(uri.LocalPath);

            }
            catch (ArgumentException) {

                // We can end up here if the path contains illegal characters (e.g. "|").
                // Even though it shouldn't be allowed, there are URLs out there that contain them.
                // We should still be able to handle this case.

                return GetFilenameWithRegex(path, isUrl: true);

            }

        }
        private static string GetFilenameWithRegex(string path, bool isUrl) {

            if (path is null)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(path))
                return "";

            string urlPathTerminals = @"?#";
            string localPathTerminals = @"?";

            string[] pathTerminals = (isUrl ? urlPathTerminals : localPathTerminals)
                .Split()
                .Select(c => Regex.Escape(c))
                .ToArray();

            Match filenameMatch = Regex.Match(path, @"(?:.*[\/\\])?(.+?)(?:$|" + string.Join("|", pathTerminals) + @")");

            return filenameMatch.Success ?
                filenameMatch.Groups[1].Value :
                "";

        }
        private static string GetRootOrScheme(string path) {

            // Returns things like "\\", "C:\", "https://", etc.

            Match rootOrSchemeMatch = Regex.Match(path, @"^(?:[^\\\/]+):(?:[\\\/]{1,2})?|^[\\\/]{2}");

            return rootOrSchemeMatch.Success ?
                rootOrSchemeMatch.Value :
                string.Empty;

        }
        private static bool StartsWithDirectorySeparatorChar(string path) {

            return !string.IsNullOrEmpty(path) &&
                (path.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString()) ||
                path.StartsWith(System.IO.Path.AltDirectorySeparatorChar.ToString()));

        }
        private static string StripRepeatedForwardSlashesAfterScheme(string path) {

            string scheme = GetScheme(path);

            if (!string.IsNullOrEmpty(scheme) && path.Length > scheme.Length)
                path = scheme + "://" + path.Substring(scheme.Length + 1).TrimStart('/');

            return path;

        }
        private static string StripRepeatedDirectorySeparators(string path) {

            path = Regex.Replace(path, $@"[{Regex.Escape(System.IO.Path.DirectorySeparatorChar.ToString())}{Regex.Escape(System.IO.Path.AltDirectorySeparatorChar.ToString())}]+", m => {

                if (m.Value.First() == System.IO.Path.DirectorySeparatorChar)
                    return System.IO.Path.DirectorySeparatorChar.ToString();
                else if (m.Value.First() == System.IO.Path.AltDirectorySeparatorChar)
                    return System.IO.Path.AltDirectorySeparatorChar.ToString();
                else
                    return m.Value;

            });

            return path;

        }

    }

}