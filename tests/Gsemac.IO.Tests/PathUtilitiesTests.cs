using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class PathUtilitiesTests {

        // GetPath

        [TestMethod]
        public void TestGetPathWithUrl() {

            Assert.AreEqual("/questions/", PathUtilities.GetPath(@"https://stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestGetPathWithDriveLetter() {

            Assert.AreEqual(@"windows\", PathUtilities.GetPath(@"C:\windows\"));

        }
        [TestMethod]
        public void TestGetPathWithEmptyString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetPath(string.Empty));

        }

        // GetRelativePath

        [TestMethod]
        public void TestGetRelativePathWithRelativePath() {

            string fullPath = @"\directory\subdirectory\file.txt";
            string relativePath = @"\directory\subdirectory";
            string result = PathUtilities.GetRelativePath(fullPath, relativePath);

            Assert.AreEqual(@"file.txt", result);

        }
        [TestMethod]
        public void TestGetRelativePathWithoutRelativePath() {

            string fullPath = @"\directory\subdirectory\file.txt";
            string relativePath = @"\directory\differentSubdirectroy";
            string result = PathUtilities.GetRelativePath(fullPath, relativePath);

            Assert.AreEqual(@"\directory\subdirectory\file.txt", result);

        }

        // GetRelativePathToRoot

        [TestMethod]
        public void TestGetPathRelativeToRootWithRoot() {

            Assert.AreEqual(@"Users\Username", PathUtilities.GetPath(@"\Users\Username"));

        }
        [TestMethod]
        public void TestGetPathRelativeToRootWithDriveRoot() {

            Assert.AreEqual(@"Users\Username", PathUtilities.GetPath(@"C:\Users\Username"));

        }
        [TestMethod]
        public void TestGetPathRelativeToRootWithNetworkShare() {

            Assert.AreEqual(@"Username", PathUtilities.GetPath(@"\\Share\Users\Username"));

        }
        [TestMethod]
        public void TestGetPathRelativeToRootWithRelativePath() {

            Assert.AreEqual(@"Users\Username", PathUtilities.GetPath(@"Users\Username"));

        }
        [TestMethod]
        public void TestGetPathRelativeToRootWithEmptyString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetPath(string.Empty));

        }

        // GetScheme

        [TestMethod]
        public void TestGetSchemeWithUrl() {

            Assert.AreEqual("https", PathUtilities.GetScheme(@"https://stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestGetSchemeWithDriveLetter() {

            Assert.AreEqual(string.Empty, PathUtilities.GetScheme(@"C:\Windows"));

        }
        [TestMethod]
        public void TestGetSchemeWithEmptyString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetScheme(string.Empty));

        }
        [TestMethod]
        public void TestGetSchemeWithInvalidCharacters() {

            Assert.AreEqual(string.Empty, PathUtilities.GetScheme(@"inv@lid://stackoverflow.com/"));

        }
        [TestMethod]
        public void TestGetSchemeWithRelativePath() {

            Assert.AreEqual(string.Empty, PathUtilities.GetScheme(@"/questions/"));

        }
        [TestMethod]
        public void TestGetSchemeWithNoColon() {

            Assert.AreEqual(string.Empty, PathUtilities.GetScheme(@"//stackoverflow.com/"));

        }
        [TestMethod]
        public void TestGetSchemeWithMultipleColons() {

            Assert.AreEqual("inval", PathUtilities.GetScheme(@"inval:d://stackoverflow.com/"));

        }
        [TestMethod]
        public void TestGetSchemeWithNoSchemeAndColonInPath() {

            Assert.AreEqual(string.Empty, PathUtilities.GetScheme(@"//website.com/path:path"));

        }
        [TestMethod]
        public void TestGetSchemeWithNoAuthority() {

            Assert.AreEqual("https", PathUtilities.GetScheme(@"https:"));

        }

        // GetRootPath

        [TestMethod]
        public void TestGetRootWithUrl() {

            Assert.AreEqual("https://stackoverflow.com", PathUtilities.GetRootPath(@"https://stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestGetRootWithRelativeUrl() {

            Assert.AreEqual(string.Empty, PathUtilities.GetRootPath(@"/?tab=interesting", new PathInfo() {
                IsUrl = true,
            }));

        }
        [TestMethod]
        public void TestGetRootWithRelativePath() {

            Assert.AreEqual(string.Empty, PathUtilities.GetRootPath(@"some/path"));

        }
        [TestMethod]
        public void TestGetRootWithRelativePathWithForwardSlash() {

            // PathUtilities.GetRootPath can't tell the difference between regular relative paths and relative URLs on its own.

            Assert.AreEqual("/", PathUtilities.GetRootPath(@"/?tab=interesting"));

        }
        [TestMethod]
        public void TestGetRootWithUrlWithBackslashes() {

            Assert.AreEqual(@"https:\\stackoverflow.com", PathUtilities.GetRootPath(@"https:\\stackoverflow.com\questions\"));

        }
        [TestMethod]
        public void TestGetRootWithUncPath() {

            Assert.AreEqual(@"\\user\documents", PathUtilities.GetRootPath(@"\\user\documents\file.txt"));

        }
        [TestMethod]
        public void TestGetRootWithDriveLetter() {

            Assert.AreEqual(@"C:", PathUtilities.GetRootPath(@"C:\Windows"));

        }
        [TestMethod]
        public void TestGetRootWithInvalidCharacters() {

            Assert.AreEqual(@"C:", PathUtilities.GetRootPath(@"C:\Wi|ndows"));

        }
        [TestMethod]
        public void TestGetRootWithAbsolutePath() {

            Assert.AreEqual(@"\", PathUtilities.GetRootPath(@"\a\b"));

        }
        [TestMethod]
        public void TestGetRootWithEmptyString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetRootPath(string.Empty));

        }

        // GetParentPath

        [TestMethod]
        public void TestGetParentPathWithUrlWithoutPath() {

            Assert.AreEqual(string.Empty, PathUtilities.GetParentPath("https://stackoverflow.com"));

        }
        [TestMethod]
        public void TestGetParentPathWithUrlEndingWithDirectorySeparator() {

            Assert.AreEqual(string.Empty, PathUtilities.GetParentPath("https://stackoverflow.com/"));

        }
        [TestMethod]
        public void TestGetParentPathWithUrlWithPath() {

            Assert.AreEqual("https://stackoverflow.com", PathUtilities.GetParentPath("https://stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestGetParentPathWithRelativePath() {

            Assert.AreEqual(@"dir", PathUtilities.GetParentPath(@"dir\subdir"));

        }

        // GetPathSegments

        [TestMethod]
        public void TestGetPathSegmentsWithUrl() {

            Assert.IsTrue(PathUtilities.GetPathSegments(@"https://stackoverflow.com/users/").SequenceEqual(new[] {
                @"https://stackoverflow.com/",
                @"users/",
            }));

        }
        [TestMethod]
        public void TestGetPathSegmentsWithLocalPath() {

            Assert.IsTrue(PathUtilities.GetPathSegments(@"c:\path\file.jpg").SequenceEqual(new[] {
                @"c:\",
                @"path\",
                "file.jpg",
            }));

        }
        [TestMethod]
        public void TestGetPathSegmentsWithUncPath() {

            Assert.IsTrue(PathUtilities.GetPathSegments(@"\\user\documents").SequenceEqual(new[] {
                @"\\user\documents",
            }));

        }

        // GetPathDepth

        [TestMethod]
        public void TestGetPathDepthWithUrl() {

            Assert.AreEqual(1, PathUtilities.GetPathDepth("https://stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestGetPathDepthWithUrlWithoutScheme() {

            // Non-rooted paths are treated like relative paths, whether or not they resemble a URL.

            Assert.AreEqual(2, PathUtilities.GetPathDepth("website.com/test/"));

        }
        [TestMethod]
        public void TestGetPathDepthWithUrlWithoutPath() {

            // Non-rooted paths are treated like relative paths, whether or not they resemble a URL.

            Assert.AreEqual(1, PathUtilities.GetPathDepth("website.com/"));

        }
        [TestMethod]
        public void TestGetPathDepthWithoutRootDomain() {

            Assert.AreEqual(4, PathUtilities.GetPathDepth("/this/is/a/path/"));

        }
        [TestMethod]
        public void TestGetPathDepthWithLocalPath() {

            Assert.AreEqual(2, PathUtilities.GetPathDepth(@"C:\Program Files\Adobe"));

        }

        // GetFileName

        [TestMethod]
        public void TestGetFilenameFromUri() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFilename("https://website.com/file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromUnrootedUri() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFilename("path/file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromUriWithUriParameters() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFilename("path/file.jpg?file=1#anchor"));

        }
        [TestMethod]
        public void TestGetFilenameFromUriWithMultipleUriParameters() {

            // The last URI parameter has its name omitted intentionally (this test is due to a real-world encounter).

            // If this were treated like a regular path, "proxy?img=file=&file.jpg" would be the filename.
            // However, the actual filename here is "proxy", which also does not have a file extension.

            Assert.AreEqual("proxy", PathUtilities.GetFilename("https://website.com/proxy?img=file=&file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromLocalPath() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFilename(@"c:\path\file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromUriWithIllegalCharacters() {

            Assert.AreEqual("|file.jpg", PathUtilities.GetFilename("path/|file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromUriWithIllegalCharactersWithHashCharacter() {

            // The hash character ("#") is a valid path character on Windows.
            // Filenames containing the hash character ("#") are valid on Windows, and the part after the hash should not be stripped.

            Assert.AreEqual("|file.jpg#hash.png", PathUtilities.GetFilename("path/|file.jpg#hash.png"));

        }
        [TestMethod]
        public void TestGetFilenameFromUriWithHashCharacter() {

            // URL fragments begin with the hash ("#") character.
            // Filenames containing the hash character ("#") are valid on Windows, and the part after the hash should not be stripped.

            Assert.AreEqual(@"PathWith#Hash.jpg", PathUtilities.GetFilename(@"C:\PathWith#Hash.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromUrlWithHashCharacter() {

            // For URLs, everything after the hash character ("#") should be ignored as it indicates the start of a URI fragment.

            Assert.AreEqual(@"PathWith.png", PathUtilities.GetFilename(@"https://example.com/PathWith.png#Hash.jpg"));

        }

        // GetFileNameWithoutExtension

        [TestMethod]
        public void TestGetFilenameWithoutExtension() {

            Assert.AreEqual("file", PathUtilities.GetFilenameWithoutExtension("path/file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameWithoutExtensionWithParameters() {

            Assert.AreEqual("file", PathUtilities.GetFilenameWithoutExtension("path/file.jpg?file=1#anchor"));

        }
        [TestMethod]
        public void TestGetFilenameWithoutExtensionWithMultiplePeriods() {

            Assert.AreEqual("file.1", PathUtilities.GetFilenameWithoutExtension("path/file.1.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameWithoutExtensionWithNoExtension() {

            Assert.AreEqual("file", PathUtilities.GetFilenameWithoutExtension("path/file"));

        }

        // GetFileExtension

        [TestMethod]
        public void TestGetFileExtensionFromUriWithIllegalCharacters() {

            Assert.AreEqual(".jpg", PathUtilities.GetFileExtension("path/|file.jpg"));

        }
        [TestMethod]
        public void TestGetFileExtensionFromUriWithExtensionInUriParameter() {

            // The last URI parameter has its name omitted intentionally (this test is due to a real-world encounter).

            // If this were treated like a regular path, "proxy?img=file=&file.jpg" would be the filename, and the extension would be ".jpg".
            // However, the actual filename here is "proxy", which does not have an extension.

            Assert.IsTrue(string.IsNullOrEmpty(PathUtilities.GetFileExtension("https://website.com/proxy?img=file=&file.jpg")));

        }
        [TestMethod]
        public void TestGetFileExtensionFromUriWithHashCharacter() {

            Assert.AreEqual(@".jpg", PathUtilities.GetFileExtension(@"C:\PathWith#Hash.jpg"));

        }
        [TestMethod]
        public void TestGetFileExtensionFromUrlWithHashCharacter() {

            Assert.AreEqual(@".png", PathUtilities.GetFileExtension(@"https://example.com/PathWith.png#Hash.jpg"));

        }
        [TestMethod]
        public void TestGetFileExtensionFromUriWithFilenameWithHashCharacter() {

            // Filenames containing the hash character ("#") are valid on Windows, and the part after the hash should not be stripped.

            Assert.AreEqual(@".jpg", PathUtilities.GetFileExtension(@"PathWith#Hash.jpg"));

        }

        // SetFileExtension

        [TestMethod]
        public void TestSetFileExtensionWithFileName() {

            Assert.AreEqual("file.jpg", PathUtilities.SetFileExtension("file.tmp", ".jpg"));

        }
        [TestMethod]
        public void TestSetFileExtensionWithRelativeFilePath() {

            Assert.AreEqual("path/file.jpg", PathUtilities.SetFileExtension("path/file.tmp", ".jpg"));

        }

        // AnonymizePath

        [TestMethod]
        public void TestAnonymizePathWithUserDirectory() {

            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = System.IO.Path.Combine(userDirectory, "test");
            string anonymizedPath = PathUtilities.AnonymizePath(path);

            Assert.AreEqual(@"%USERPROFILE%\test", anonymizedPath);

        }

        // SanitizePath

        [TestMethod]
        public void TestSanitizePathWithRootedPath() {

            // SanitizePath attempts to maintain the directory structure by default.

            Assert.AreEqual(@"C:\Users\Admin\Documents", PathUtilities.SanitizePath(@"C:\Users\Admin\Documents"));

        }
        [TestMethod]
        public void TestSanitizePathWithRootedPathPreservesLeadingDirectorySeparator() {

            Assert.AreEqual(@"\Users\Admin\Documents", PathUtilities.SanitizePath(@"\Users\Admin\Documents"));

        }
        [TestMethod]
        public void TestSanitizePathWithRelativePath() {

            Assert.AreEqual(@"Users\Admin\Documents", PathUtilities.SanitizePath(@"Users\Admin\Documents"));

        }
        [TestMethod]
        public void TestSanitizePathWithReplacement() {

            Assert.AreEqual(@"C++Users+Admin+Documents", PathUtilities.SanitizePath(@"C:\Users\Admin\Documents", "+", SanitizePathOptions.StripInvalidChars));

        }
        [TestMethod]
        public void TestSanitizePathWithEmptyReplacement() {

            Assert.AreEqual(@"CUsersAdminDocuments", PathUtilities.SanitizePath(@"C:\Users\Admin\Documents", string.Empty, SanitizePathOptions.StripInvalidChars));

        }
        [TestMethod]
        public void TestSanitizePathWithReplacementEvaluatorDelegate() {

            Assert.AreEqual(@"C++Users+Admin+Documents", PathUtilities.SanitizePath(@"C:\Users\Admin\Documents", _ => "+", SanitizePathOptions.StripInvalidChars));

        }
        [TestMethod]
        public void TestSanitizePathWithEquivalentValidPathChars() {

            Assert.AreEqual(@"“C∶＼Users＼Admin＼Documents”",
                PathUtilities.SanitizePath(@"""C:\Users\Admin\Documents""", new SanitizePathOptions() {
                    UseEquivalentValidPathChars = true,
                }));

        }
        [TestMethod]
        public void TestSanitizePathWithUncPathPreservesLeadingForwardSlashes() {

            Assert.AreEqual(@"\\user\documents", PathUtilities.SanitizePath(@"\\user\documents"));

        }
        [TestMethod]
        public void TestSanitizePathWithUrlPreservesForwardSlashesAfterScheme() {

            Assert.AreEqual(@"https://stackoverflow.com/questions/", PathUtilities.SanitizePath(@"https://stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestSanitizePathWithUrlStripsExcessForwardSlashesAfterScheme() {

            Assert.AreEqual(@"https://stackoverflow.com/questions/", PathUtilities.SanitizePath(@"https://///////stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestSanitizePathWithInvalidCharacters() {

            Assert.AreEqual(@"\documents\", PathUtilities.SanitizePath(@"\documents|\"));

        }

        // NormalizeDirectorySeparators

        [TestMethod]
        public void TestNormalizeDirectorySeparatorsWithLocalPath() {

            Assert.AreEqual(@"C:\Users\Admin\Documents", PathUtilities.NormalizeDirectorySeparators(@"C:\Users/Admin/Documents"));

        }
        [TestMethod]
        public void TestNormalizeDirectorySeparatorsWithUrl() {

            Assert.AreEqual(@"https://stackoverflow.com/", PathUtilities.NormalizeDirectorySeparators(@"https://stackoverflow.com\"));

        }

        // NormalizeDotSegments

        [TestMethod]
        public void TestNormalizeDotSegmentsWithRootedPathWithCurrentPathSegments() {

            // "./" segments can essentially just be stripped to get the actual path.
            // Under normal circumstances, they would appear at the beginning of relative paths.

            Assert.AreEqual(@"https://example.com/test1/test2/", PathUtilities.NormalizeDotSegments(@"https://example.com/./test1/././test2/./"));

        }
        [TestMethod]
        public void TestNormalizeDotSegmentsWithRelativePathWithCurrentPathSegments() {

            Assert.AreEqual(@"test1/test2/", PathUtilities.NormalizeDotSegments(@"./test1/./test2/"));

        }
        [TestMethod]
        public void TestNormalizeDotSegmentsWithRootedPathWithParentPathSegments() {

            // Note that using "../" multiple times on a rooted path just brings us back to the root, never "above" it.

            Assert.AreEqual(@"https://example.com/test2/", PathUtilities.NormalizeDotSegments(@"https://example.com/../../test1/../test2/"));

        }
        [TestMethod]
        public void TestNormalizeDotSegmentsWithRelativePathWithParentPathSegments() {

            // For relative paths with no root, allow moving "up" indefinitely (basically, the root is the empty string).
            // NormalizeDotSegments should ideally be called only on complete paths.

            Assert.AreEqual(@"test2/", PathUtilities.NormalizeDotSegments(@"../test1/../../test2/"));

        }
        [TestMethod]
        public void TestNormalizeDotSegmentsWithUncPathRetainsRootPath() {

            Assert.AreEqual(@"\\user\documents\test2\", PathUtilities.NormalizeDotSegments(@"\\user\documents\..\..\test1\..\..\..\..\..\test2\"));

        }

        // IsLocalPath

        [TestMethod]
        public void TestIsLocalPathWithAbsoluteFilePathWithDriveLetter() {

            Assert.IsTrue(PathUtilities.IsLocalPath(@"C:\user\documents\file.txt", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithAbsoluteDirectoryPathWithDriveLetter() {

            Assert.IsTrue(PathUtilities.IsLocalPath(@"C:\user\documents\", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithAbsoluteDirectoryPath() {

            // The path "\user\documents\" could be:
            // - A relative path as part of a local or remote path.
            // - A rooted local path.
            // We'll assume that it is a rooted local path rather than treating it as indeterminate.

            Assert.IsTrue(PathUtilities.IsLocalPath(@"\user\documents\", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithRelativeDirectoryPath() {

            Assert.IsTrue(PathUtilities.IsLocalPath(@"documents\", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithAbsoluteDirectoryPathWithInvalidCharacters() {

            // See TestIsLocalPathWithAbsoluteDirectoryPath for discussion.
            // IsLocalPath does not attempt to do any validation.

            Assert.IsTrue(PathUtilities.IsLocalPath(@"\documents|\", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithRelativeDirectoryPathWithInvalidCharacters() {

            // IsLocalPath does not attempt to do any validation.

            Assert.IsTrue(PathUtilities.IsLocalPath(@"documents|\", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithUrl() {

            Assert.IsFalse(PathUtilities.IsLocalPath(@"https://example.com", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithUrlWithMissingProtocol() {

            // UNC paths (network paths) are not considered local.
            // The "Uri" class considers "file://" and "\\" both valid for UNC paths.

            Assert.IsFalse(PathUtilities.IsLocalPath(@"//example.com", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithUncFilePath() {

            Assert.IsFalse(PathUtilities.IsLocalPath(@"\\user\documents\file.txt", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithLongPathSyntax() {

            Assert.IsTrue(PathUtilities.IsLocalPath(@"\\?\C:\documents\file.txt", verifyPathExists: false));

        }

        // IsPathRooted

        [TestMethod]
        public void TestIsPathRootedWithAbsoluteDirectoryPath() {

            Assert.IsTrue(PathUtilities.IsPathRooted(@"\user\documents\"));

        }
        [TestMethod]
        public void TestIsPathRootedWithAbsoluteDirectoryPathWithDriverLetter() {

            Assert.IsTrue(PathUtilities.IsPathRooted(@"C:\user\documents\"));

        }
        [TestMethod]
        public void TestIsPathRootedWithUrl() {

            Assert.IsTrue(PathUtilities.IsPathRooted(@"https://example.com"));

        }
        [TestMethod]
        public void TestIsPathRootedWithUncPath() {

            Assert.IsTrue(PathUtilities.IsPathRooted(@"\\user\documents\"));

        }
        [TestMethod]
        public void TestIsPathRootedWithRelativeDirectoryPath() {

            Assert.IsFalse(PathUtilities.IsPathRooted(@"documents\"));

        }
        [TestMethod]
        public void TestIsPathRootedWithLongAbsoluteDirectoryPath() {

            string path = @"C:\user\documents\".PadRight(PathUtilities.MaxDirectoryPathLength, '0') + @"\";

            Assert.IsTrue(PathUtilities.IsPathRooted(path));

        }
        [TestMethod]
        public void TestIsPathRootedWithLongAbsoluteDirectoryPathWithExtendedLengthPrefix() {

            string path = PathUtilities.ExtendedLengthPrefix + @"C:\user\documents\".PadRight(PathUtilities.MaxDirectoryPathLength, '0') + @"\";

            Assert.IsTrue(PathUtilities.IsPathRooted(path));

        }

        // IsPathTooLong

        [TestMethod]
        public void TestIsPathTooLongWithDirectoryPathEqualToMaxDirectoryPathLength() {

            // Note that the ending directory separator is not considered when calculating the length.

            string path = @"C:\a\b\c\".PadRight(PathUtilities.MaxDirectoryPathLength, '0') + @"\";

            Assert.IsFalse(PathUtilities.IsPathTooLong(path));

        }
        [TestMethod]
        public void TestIsPathTooLongWithDirectoryPathLongerThanMaxDirectoryPathLength() {

            string path = @"C:\a\b\c\".PadRight(PathUtilities.MaxDirectoryPathLength + 1, '0') + @"\";

            Assert.IsTrue(PathUtilities.IsPathTooLong(path));

        }
        [TestMethod]
        public void TestIsPathTooLongWithDirectoryPathEqualToMaxDirectoryPathLengthWithInvalidPathCharacter() {

            // Note that the ending directory separator is not considered when calculating the length.

            string path = @"C:\a\b\c|\".PadRight(PathUtilities.MaxDirectoryPathLength, '0') + @"\";

            Assert.IsFalse(PathUtilities.IsPathTooLong(path));

        }
        [TestMethod]
        public void TestIsPathTooLongWithDirectoryPathLongerThanMaxDirectoryPathLengthWithInvalidPathCharacter() {

            string path = @"C:\a\b\c|\".PadRight(PathUtilities.MaxDirectoryPathLength + 1, '0') + @"\";

            Assert.IsTrue(PathUtilities.IsPathTooLong(path));

        }
        [TestMethod]
        public void TestIsPathTooLongWithFilePathEqualToMaxFilePathLength() {

            string path = @"C:\a\b\c\".PadRight(PathUtilities.MaxFilePathLength, '0');

            Assert.IsFalse(PathUtilities.IsPathTooLong(path));

        }
        [TestMethod]
        public void TestIsPathTooLongWithFilePathLongerThanMaxFilePathLength() {

            string path = @"C:\a\b\c\".PadRight(PathUtilities.MaxFilePathLength + 1, '0');

            Assert.IsTrue(PathUtilities.IsPathTooLong(path));

        }

        // IsSubpathOf

        [TestMethod]
        public void TestIsSubpathOfWithPathAndRootedPath() {

            // If the second path is rooted, it must start with the entirety of the parent path in order to be a subpath.

            Assert.IsFalse(PathUtilities.IsSubpathOf("/1/2/3/", "/3/4"));

        }
        [TestMethod]
        public void TestIsSubpathOfWithPathAndRootedSubpath() {

            Assert.IsTrue(PathUtilities.IsSubpathOf("/1/2/3/", "/1/2/3/4/"));

        }
        [TestMethod]
        public void TestIsSubpathOfWithPathAndSamePath() {

            Assert.IsFalse(PathUtilities.IsSubpathOf("/1/2/3/", "/1/2/3/"));

        }
        [TestMethod]
        public void TestIsSubpathOfWithPathAndSamePathWithoutLeadingDirectorySeparator() {

            Assert.IsFalse(PathUtilities.IsSubpathOf("/1/2/3/", "1/2/3/"));

        }
        [TestMethod]
        public void TestIsSubpathOfWithPathAndSamePathWithoutTrailingDirectorySeparator() {

            Assert.IsFalse(PathUtilities.IsSubpathOf("/1/2/3/", "/1/2/3"));

        }
        [TestMethod]
        public void TestIsSubpathOfWithUrlAndSubpath() {

            Assert.IsTrue(PathUtilities.IsSubpathOf("https://example.com/1", "https://example.com/1/2"));

        }

        // PathContainsSegment

        [TestMethod]
        public void TestPathContainsSegmentReturnsTrueWithDirectorySegment() {

            Assert.IsTrue(PathUtilities.PathContainsSegment(@"C:\path\to\directory\", "to"));

        }
        [TestMethod]
        public void TestPathContainsSegmentReturnsFalseWithDirectorySegment() {

            Assert.IsFalse(PathUtilities.PathContainsSegment(@"C:\path\to\directory\", "too"));

        }

        // AreEqual

        [TestMethod]
        public void TestAreEqualWithEqualPathsWithSameDirectorySeparator() {

            Assert.IsTrue(PathUtilities.AreEqual(@"C:\path\to\directory\", @"C:\path\to\directory\"));

        }
        [TestMethod]
        public void TestAreEqualWithEqualPathsWithDifferentDirectorySeparator() {

            Assert.IsTrue(PathUtilities.AreEqual(@"C:/path/to/directory/", @"C:\path\to\directory\"));

        }
        [TestMethod]
        public void TestAreEqualWithEqualPathsWithMixedDirectorySeparator() {

            Assert.IsTrue(PathUtilities.AreEqual(@"C:/path\to/directory\", @"C:/path/to\directory\"));

        }
        [TestMethod]
        public void TestAreEqualWithEqualPathsWithMixedCase() {

            Assert.IsTrue(PathUtilities.AreEqual(@"C:\path\TO\directory\", @"C:\path\to\DIRECTORY\"));

        }
        [TestMethod]
        public void TestAreEqualWithUnequalPaths() {

            Assert.IsFalse(PathUtilities.AreEqual(@"C:\path\to\", @"C:\path\to\directory\"));

        }
        [TestMethod]
        public void TestAreEqualWithBothPathsEmpty() {

            Assert.IsTrue(PathUtilities.AreEqual(string.Empty, string.Empty));

        }
        [TestMethod]
        public void TestAreEqualWithFirstPathEmpty() {

            Assert.IsFalse(PathUtilities.AreEqual(string.Empty, @"C:\path\to\directory\"));

        }
        [TestMethod]
        public void TestAreEqualWithSecondPathEmpty() {

            Assert.IsFalse(PathUtilities.AreEqual(@"C:\path\to\directory\", string.Empty));

        }

    }

}