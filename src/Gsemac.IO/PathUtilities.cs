using Gsemac.IO.Properties;
using Gsemac.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

            return GetPath(path, PathInfo.Default);

        }
        public static string GetPath(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            string relativePath = path;

            if (!string.IsNullOrWhiteSpace(relativePath))
                relativePath = path.Substring(GetRootPath(path).Length);

            if ((pathInfo.IsUrl ?? false) || IsUrl(path)) {

                // Strip query and fragment strings.

                relativePath = StringUtilities.BeforeLast(StringUtilities.BeforeLast(relativePath, "?"), "#");

            }
            else {

                relativePath = TrimLeftDirectorySeparators(relativePath);

            }

            return relativePath;

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

            IEnumerable<string> remainingSegments = StringUtilities.Split(path, new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, new StringSplitOptionsEx() {
                SplitAfterDelimiter = true,
            }).Where(segment => !string.IsNullOrEmpty(segment));

            foreach (string segment in remainingSegments)
                yield return segment;

        }

        public static string GetRelativePath(string path, string basePath) {

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(basePath))
                return path;

            string fullInputPath = Path.GetFullPath(path);
            string fullRelativeToPath = TrimDirectorySeparators(Path.GetFullPath(basePath)) + Path.DirectorySeparatorChar;

            int index = fullInputPath.IndexOf(fullRelativeToPath);

            if (index >= 0)
                return fullInputPath.Substring(index + fullRelativeToPath.Length, fullInputPath.Length - (index + fullRelativeToPath.Length));
            else
                return path;

        }

        public static string GetRootPath(string path) {

            return GetRootPath(path, PathInfo.Default);

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
                        rootPath = Path.GetPathRoot(path);

                }

            }

            if (!string.IsNullOrEmpty(rootPath) && !rootPath.All(c => c.Equals(Path.DirectorySeparatorChar) || c.Equals(Path.AltDirectorySeparatorChar)))
                rootPath = TrimRightDirectorySeparators(rootPath);

            return rootPath;

        }

        public static string GetParentPath(string path) {

            return GetParentPath(path, PathInfo.Default);

        }
        public static string GetParentPath(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            string rootPath = GetRootPath(path, pathInfo);
            int separatorIndex = TrimRightDirectorySeparators(path).LastIndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });

            if (separatorIndex < rootPath.Length)
                return string.Empty;

            return path.Substring(0, separatorIndex);

        }

        public static int GetPathDepth(string path) {

            return GetPathDepth(path, PathDepthOptions.Default);

        }
        public static int GetPathDepth(string path, IPathDepthOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(path))
                return 0;

            path = NormalizeDirectorySeparators(TrimLeftDirectorySeparators(GetPath(path)));

            if (options.IgnoreTrailingDirectorySeparators)
                path = TrimRightDirectorySeparators(path);

            if (string.IsNullOrWhiteSpace(path))
                return 0;

            return path.Split(new[] { Path.DirectorySeparatorChar }).Count();

        }

        public static string GetScheme(string path) {

            path = path?.Trim() ?? string.Empty;

            Match match = Regex.Match(path, @"^(?<scheme>[\w][\w+-.]+):");

            if (match.Success)
                return match.Groups["scheme"].Value;

            return string.Empty;

        }
        public static string SetScheme(string path, string scheme) {

            path = path?.Trim() ?? string.Empty;
            string schemePart;

            if (!string.IsNullOrWhiteSpace(scheme)) {

                schemePart = $"{scheme.Trim()}://";

            }
            else {

                // If the scheme is empty, we'll use a relative scheme.

                schemePart = "//";

            }

            Regex regex = new Regex(@"^(?:[\w][\w+-.]+:)?(?:\/\/)?");

            return regex.Replace(path, schemePart, 1);

        }

        public static string GetFileName(string path) {

            return GetFileName(path, PathInfo.Default);

        }
        public static string GetFileName(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            // This process should work for both remote and local paths.
            // The user is optionally able to specify explicitly whether the path is a URL or a local path.

            if (IsUrl(path, pathInfo))
                return GetFileNameFromUrl(path);

            // If the path cannot be determined to be a URL, treat it like a local path.
            // Invalid path characters are be allowed (e.g. "|"), and content after the hash character ("#") should be included in the filename.
            // While this signifies the start of a URI fragment for URLs, it is a valid path character on Windows and most Linux flavors.

            return GetFileNameWithRegex(path, IsUrl(path, pathInfo));

        }
        public static string GetFileNameWithoutExtension(string path) {

            string fileName = GetFileName(path);

            return StringUtilities.BeforeLast(fileName, ".");

        }

        public static string GetFileExtension(string path) {

            string filename = GetFileName(path);

            if (string.IsNullOrWhiteSpace(filename))
                return string.Empty;
            else
                return Path.GetExtension(ReplaceInvalidPathChars(filename, SanitizePathOptions.Default));

        }
        public static string SetFileExtension(string path, string extension) {

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(extension));

            if (!IsFilePath(path, verifyFileExists: false))
                throw new ArgumentException(string.Format(ExceptionMessages.PathIsNotFilePath, path));

            extension = extension?.Trim();

            if (!string.IsNullOrWhiteSpace(extension) && !extension.StartsWith("."))
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
        public static bool HasFileExtension(string path) {

            if (string.IsNullOrWhiteSpace(path))
                return false;

            return !string.IsNullOrWhiteSpace(GetFileExtension(path));

        }

        public static long GetSize(string path) {

            if (TryGetSize(path, out long size))
                return size;

            return 0;

        }
        public static bool TryGetSize(string path, out long size) {

            size = 0;

            if (string.IsNullOrWhiteSpace(path))
                return false;

            if (File.Exists(path) && FileUtilities.TryGetSize(path, out size))
                return true;

            if (Directory.Exists(path) && DirectoryUtilities.TryGetSize(path, out size))
                return true;

            return false;

        }

        public static string GetTemporaryDirectoryPath() {

            return GetTemporaryDirectoryPath(TemporaryPathOptions.Default);

        }
        public static string GetTemporaryDirectoryPath(ITemporaryPathOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (!options.EnsureUnique)
                return GetTemporaryFilePath(options);

            lock (uniquePathMutex) {

                string result;

                // Theoretically, this could result in an infinite loop. But that won't happen... Right?

                do {

                    // "EnsureUnique" is set to false so that a file isn't created, which would prevent us from creating the directory.

                    result = GetTemporaryFilePath(new TemporaryPathOptions() {
                        EnsureUnique = false,
                    });

                } while (Directory.Exists(result));

                Directory.CreateDirectory(result);

                return result;

            }

        }

        public static string GetTemporaryFilePath() {

            return GetTemporaryFilePath(TemporaryPathOptions.Default);

        }
        public static string GetTemporaryFilePath(ITemporaryPathOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return options.EnsureUnique ?
                 Path.GetTempFileName() :
                 Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        }

        public static bool IsTemporaryFilePath(string path) {

            bool isTemporaryFilePath = false;

            try {

                path = Path.GetFullPath(path);

                isTemporaryFilePath = path.StartsWith(Path.GetTempPath()) && IsFilePath(path);

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

                    path = Path.Combine("%USERPROFILE%", GetRelativePath(path, userDirectory));

                }

            }

            return path;

        }

        public static string SanitizePath(string path) {

            return SanitizePath(path, SanitizePathOptions.Default);

        }
        public static string SanitizePath(string path, ISanitizePathOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.UseEquivalentValidPathChars) {

                bool inQuotes = false;

                return SanitizePath(path, c => GetEquivalentValidPathChar(c, ref inQuotes), options);

            }
            else {

                return SanitizePath(path, string.Empty, options);

            }

        }
        public static string SanitizePath(string path, string replacement) {

            return SanitizePath(path, replacement, SanitizePathOptions.Default);

        }
        public static string SanitizePath(string path, string replacement, ISanitizePathOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return SanitizePath(path, c => replacement, options);

        }
        public static string SanitizePath(string path, CharReplacementEvaluatorDelegate replacementEvaluator) {

            if (replacementEvaluator is null)
                throw new ArgumentNullException(nameof(replacementEvaluator));

            return SanitizePath(path, replacementEvaluator, SanitizePathOptions.Default);

        }
        public static string SanitizePath(string path, CharReplacementEvaluatorDelegate replacementEvaluator, ISanitizePathOptions options) {

            if (replacementEvaluator is null)
                throw new ArgumentNullException(nameof(replacementEvaluator));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return ReplaceInvalidPathChars(path, replacementEvaluator, options);

        }

        public static string TrimDirectorySeparators(string path) {

            return path?.Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        }
        public static string TrimLeftDirectorySeparators(string path) {

            return path?.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        }
        public static string TrimRightDirectorySeparators(string path) {

            return path?.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        }

        public static string NormalizeDirectorySeparators(string path) {

            return NormalizeDirectorySeparators(path, PathInfo.Default);

        }
        public static string NormalizeDirectorySeparators(string path, char directorySeparatorChar) {


            path = string.Join(directorySeparatorChar.ToString(),
                path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            return path;

        }
        public static string NormalizeDirectorySeparators(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            if (string.IsNullOrEmpty(path))
                return path;

            char directorySeparatorChar = Path.DirectorySeparatorChar;

            if (IsUrl(path, pathInfo))
                directorySeparatorChar = '/';

            return NormalizeDirectorySeparators(path, directorySeparatorChar);

        }

        public static bool IsDirectorySeparator(char value) {

            return IsDirectorySeparator(value.ToString(CultureInfo.InvariantCulture));

        }
        public static bool IsDirectorySeparator(string value) {

            return value.Equals(Path.DirectorySeparatorChar) ||
                value.Equals(Path.AltDirectorySeparatorChar);

        }

        public static string NormalizeDotSegments(string path) {

            return NormalizeDotSegments(path, PathInfo.Default);

        }
        public static string NormalizeDotSegments(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            if (string.IsNullOrWhiteSpace(path))
                return path;

            string currentPathSegmentStr1 = "." + Path.DirectorySeparatorChar;
            string currentPathSegmentStr2 = "." + Path.AltDirectorySeparatorChar;
            string parentPathSegmentStr1 = ".." + Path.DirectorySeparatorChar;
            string parentPathSegmentStr2 = ".." + Path.AltDirectorySeparatorChar;

            bool isPathRooted = IsPathRooted(path, pathInfo);

            Stack<string> segments = new Stack<string>();

            foreach (string segment in GetPathSegments(path)) {

                if (segment.Equals(currentPathSegmentStr1) || segment.Equals(currentPathSegmentStr2))
                    continue;

                if (segment.Equals(parentPathSegmentStr1) || segment.Equals(parentPathSegmentStr2)) {

                    // Don't allow the root of the path to be popped off.

                    if (segments.Count > 0 && (segments.Count > 1 || !isPathRooted))
                        segments.Pop();

                }
                else {

                    segments.Push(segment);

                }

            }

            // Reverse the segments before joining, because iterating through a stack takes us from top to bottom.

            return string.Join(string.Empty, segments.Reverse());

        }

        public static bool IsFilePath(string path, bool verifyFileExists = false) {

            bool result = path.Any() &&
                path.Last() != Path.DirectorySeparatorChar &&
                path.Last() != Path.AltDirectorySeparatorChar;

            if (verifyFileExists && result)
                result = File.Exists(path);

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
                else if (new char[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar }.Any(c => c == path.First())) {

                    // Rooted paths that don't specify a scheme will be considered local.

                    isLocalPath = !verifyPathExists || PathExists(testUri.LocalPath);

                }
                else if (Uri.TryCreate(path, UriKind.Relative, out _)) {

                    // Check the full path for this relative path.

                    path = Path.GetFullPath(path);

                    isLocalPath = IsLocalPath(path, verifyPathExists: verifyPathExists);

                }

            }

            return isLocalPath;

        }

        public static bool IsPathRooted(string path) {

            return IsPathRooted(path, PathInfo.Default);

        }
        public static bool IsPathRooted(string path, IPathInfo pathInfo) {

            if (pathInfo is null)
                throw new ArgumentNullException(nameof(pathInfo));

            // "System.IO.Path.IsPathRooted" throws an exception for paths longer than the maximum path length, as well as for malformed paths (e.g. paths containing invalid characters).

            // URLs starting with path separators are not rooted, but relative to the root.

            if (IsUrl(path, pathInfo) && (path.StartsWith("/") || path.StartsWith("\\")))
                return false;

            string directorySeparatorsStr = Path.DirectorySeparatorChar.ToString() + Path.AltDirectorySeparatorChar.ToString();
            string pattern = @"^(?:[" + Regex.Escape(directorySeparatorsStr) + @"]|[a-z]+\:\/\/|[a-z]\:)";

            return Regex.IsMatch(path, pattern, RegexOptions.IgnoreCase);

        }

        public static bool IsPathTooLong(string path) {

            // For the purpose of checking the length, replace all illegal characters in the path.
            // This will ensure Path methods don't throw.

            path = ReplaceInvalidPathChars(path, " ", new SanitizePathOptions() {
                PreserveDirectoryStructure = true,
            });

            path = Path.GetFullPath(path);

            // Check the length of the entire path.

            if (IsFilePath(path, false)) {

                if (Path.GetDirectoryName(path).Length > MaxDirectoryPathLength || path.Length > MaxFilePathLength)
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

        public static bool IsSubpathOf(string parentPath, string childPath) {

            if (string.IsNullOrWhiteSpace(parentPath) || string.IsNullOrWhiteSpace(childPath))
                return false;

            if (!IsPathRooted(parentPath))
                parentPath = Path.GetFullPath(parentPath);

            if (!IsPathRooted(childPath))
                childPath = Path.GetFullPath(childPath);

            parentPath = NormalizeDirectorySeparators(TrimRightDirectorySeparators(parentPath) + "/");
            childPath = NormalizeDirectorySeparators(TrimRightDirectorySeparators(childPath) + "/");

            return !AreEqual(parentPath, childPath) &&
                childPath.StartsWith(parentPath);

        }

        public static bool PathExists(string path) {

            return Directory.Exists(path) || File.Exists(path);

        }

        public static bool PathContainsSegment(string path, string pathSegment) {

            pathSegment = TrimDirectorySeparators(pathSegment);

            return GetPathSegments(path)
                .Select(segment => TrimDirectorySeparators(segment))
                .Any(segment => segment.Equals(pathSegment, StringComparison.OrdinalIgnoreCase));

        }

        public static bool AreEqual(string path1, string path2) {

            return AreEqual(path1, path2, StringComparison.OrdinalIgnoreCase);

        }
        public static bool AreEqual(string path1, string path2, StringComparison stringComparison) {

            // This is intended to be a simple comparison between two paths that disregards equivalent path separators.
            // For a more robust approach that considers full path equivalency, use the "AreEquivalent" method.

            // On both Windows and Unix-based systems, trailing directory separators are irrelevant.
            // Trailing directory separators are typically used to denote directory paths, but we cannot have a directory and file with the same name.
            // For this reason, the two paths must be equivalent to each other.
            // https://unix.stackexchange.com/q/22447

            // URLs with trailing slashes are not treated the same as local paths with trailing slashes.
            // It's up to the web server to decide how to interpret them, and there are certainly cases where the two paths are not equivalent to each other.
            // https://stackoverflow.com/q/942751

            if (!IsUrl(path1))
                path1 = path1?.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (!IsUrl(path2))
                path2 = path2?.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // If the paths are both empty, consider the paths to be equal.

            if (string.IsNullOrWhiteSpace(path1) && string.IsNullOrWhiteSpace(path2))
                return true;

            // If only one of the paths is empty, the paths are not equal.

            if (string.IsNullOrWhiteSpace(path1) || string.IsNullOrWhiteSpace(path2))
                return false;

            // On Windows systems, we should normalize directory separators because "/" and "\" are equivalent.
            // However, on Unix systems and in URLs, the only valid directory separator is "/". Should we account for this?

            path1 = NormalizeDirectorySeparators(path1);
            path2 = NormalizeDirectorySeparators(path2);

            return path1.Equals(path2, stringComparison);

        }
        public static bool AreEquivalent(string path1, string path2) {

            return AreEquivalent(path1, path2, StringComparison.OrdinalIgnoreCase);

        }
        public static bool AreEquivalent(string path1, string path2, StringComparison stringComparison) {

            // Extra slashes after the scheme are stripped by major web browsers and clients, so the two paths should point to the same location.
            // This isn't technically correct, but is common enough behavior that most clients implement it.
            // https://github.com/curl/curl/issues/791

            if (IsUrl(path1))
                path1 = StripRepeatedForwardSlashesAfterScheme(path1);

            if (IsUrl(path2))
                path2 = StripRepeatedForwardSlashesAfterScheme(path2);

            // If the two paths are equal, consider them to also be equivalent.

            if (AreEqual(path1, path2, stringComparison))
                return true;

            // To decide if two paths are equivalent, we need to fully expand them.
            // It's possible for "GetFullPath" to throw if we pass in an invalid path.
            // If it throws for either path, consider them not to be equivalent.

            try {

                path1 = Path.GetFullPath(path1);
                path2 = Path.GetFullPath(path2);

                return AreEqual(path1, path2, stringComparison);

            }
            catch (Exception) {

                return false;

            }

        }

        // Private members

        private static readonly object uniquePathMutex = new object();

        private static bool EndsWithDirectorySeparatorChar(string path) {

            return !string.IsNullOrEmpty(path) &&
                (path.EndsWith(Path.DirectorySeparatorChar.ToString()) ||
                path.EndsWith(Path.AltDirectorySeparatorChar.ToString()));

        }
        private static string ReplaceInvalidPathChars(string path, ISanitizePathOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return ReplaceInvalidPathChars(path, string.Empty, options);

        }
        private static string ReplaceInvalidPathChars(string path, string replacement, ISanitizePathOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return ReplaceInvalidPathChars(path, c => replacement, options);

        }
        private static string ReplaceInvalidPathChars(string path, CharReplacementEvaluatorDelegate replacementEvaluator, ISanitizePathOptions options) {

            if (replacementEvaluator is null)
                throw new ArgumentNullException(nameof(replacementEvaluator));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            string rootOrScheme = string.Empty;
            IEnumerable<char> invalidCharacters = Enumerable.Empty<char>();

            // To match the behavior of popular web browsers, trim excess forward slashes after the scheme when using HTTP/HTTPS.
            // https://github.com/whatwg/url/issues/118
            // This is only done when the "PreserveDirectoryStructure" flag is toggled, because otherwise the slashes are replaced anyway (as may be desired).

            if (options.PreserveDirectoryStructure)
                StripRepeatedForwardSlashesAfterScheme(path);

            // Normalize directory separators.

            if (options.NormalizeDirectorySeparators)
                path = NormalizeDirectorySeparators(path);

            if (options.StripInvalidPathChars)
                invalidCharacters = invalidCharacters.Concat(Path.GetInvalidPathChars());

            if (options.StripInvalidFileNameChars)
                invalidCharacters = invalidCharacters.Concat(Path.GetInvalidFileNameChars());

            if (options.PreserveDirectoryStructure) {

                // The root of the path might contain characters that would be invalid file name characters (e.g. ':' in "C:\").
                // In order to preserve the root path information, we'll remove it for now and add it back later.

                rootOrScheme = GetRootOrScheme(path);

                if (!string.IsNullOrEmpty(rootOrScheme))
                    path = path.Substring(rootOrScheme.Length);

                invalidCharacters = invalidCharacters.Where(c => c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar);

            }

            // Strip repeated directory separators.

            if (options.StripRepeatedDirectorySeparators)
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
        private static string GetFileNameFromUrl(string path) {

            if (path is null)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(path))
                return "";

            try {

                if (!Uri.TryCreate(path, UriKind.Absolute, out Uri uri))
                    uri = new Uri(new Uri("http://anything"), path);

                return Path.GetFileName(uri.LocalPath);

            }
            catch (ArgumentException) {

                // We can end up here if the path contains illegal characters (e.g. "|").
                // Even though it shouldn't be allowed, there are URLs out there that contain them.
                // We should still be able to handle this case.

                return GetFileNameWithRegex(path, isUrl: true);

            }

        }
        private static string GetFileNameWithRegex(string path, bool isUrl) {

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
                (path.StartsWith(Path.DirectorySeparatorChar.ToString()) ||
                path.StartsWith(Path.AltDirectorySeparatorChar.ToString()));

        }
        private static string StripRepeatedForwardSlashesAfterScheme(string path) {

            string scheme = GetScheme(path);

            if (!string.IsNullOrEmpty(scheme) && path.Length > scheme.Length)
                path = scheme + "://" + path.Substring(scheme.Length + 1).TrimStart('/');

            return path;

        }
        private static string StripRepeatedDirectorySeparators(string path) {

            path = Regex.Replace(path, $@"[{Regex.Escape(Path.DirectorySeparatorChar.ToString())}{Regex.Escape(Path.AltDirectorySeparatorChar.ToString())}]+", m => {

                if (m.Value.First() == Path.DirectorySeparatorChar)
                    return Path.DirectorySeparatorChar.ToString();
                else if (m.Value.First() == Path.AltDirectorySeparatorChar)
                    return Path.AltDirectorySeparatorChar.ToString();
                else
                    return m.Value;

            });

            return path;

        }

    }

}