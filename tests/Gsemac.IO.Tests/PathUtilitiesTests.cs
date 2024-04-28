using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
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
        public void TestGetPathWithUrlWithQueryString() {

            Assert.AreEqual("/questions/", PathUtilities.GetPath(@"https://stackoverflow.com/questions/?name=value"));

        }
        [TestMethod]
        public void TestGetPathWithUrlWithFragment() {

            Assert.AreEqual("/questions/", PathUtilities.GetPath(@"https://stackoverflow.com/questions/#fragment"));

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
        public void TestGetSchemeWithNullString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetScheme(null));

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

        // SetScheme

        [TestMethod]
        public void TestSetSchemeWithPreExistingScheme() {

            Assert.AreEqual("http://stackoverflow.com", PathUtilities.SetScheme(@"https://stackoverflow.com", "http"));

        }
        [TestMethod]
        public void TestSetSchemeWithRelativeScheme() {

            Assert.AreEqual("https://stackoverflow.com", PathUtilities.SetScheme(@"//stackoverflow.com", "https"));

        }
        [TestMethod]
        public void TestSetSchemeWithoutPreExistingScheme() {

            Assert.AreEqual("https://stackoverflow.com", PathUtilities.SetScheme(@"stackoverflow.com", "https"));

        }
        [TestMethod]
        public void TestSetSchemeWithoutPathBeginningWithDirectorySeparator() {

            // Valid "file://" URIs can begin with a directory separator character.
            // https://superuser.com/a/352134/1762496

            Assert.AreEqual("file:///path", PathUtilities.SetScheme(@"/path", "file"));

        }
        [TestMethod]
        public void TestSetSchemeWithEmptyScheme() {

            // Setting the scheme to an empty string should completely remove the scheme.

            Assert.AreEqual("//stackoverflow.com", PathUtilities.SetScheme(@"https://stackoverflow.com", string.Empty));

        }
        [TestMethod]
        public void TestSetSchemeWithNullScheme() {

            Assert.AreEqual("//stackoverflow.com", PathUtilities.SetScheme(@"https://stackoverflow.com", null));

        }
        [TestMethod]
        public void TestSetSchemeWithEmptyPath() {

            Assert.AreEqual("https://", PathUtilities.SetScheme(string.Empty, "https"));

        }
        [TestMethod]
        public void TestSetSchemeWithNullPath() {

            Assert.AreEqual("https://", PathUtilities.SetScheme(null, "https"));

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
        public void TestGetFileNameFromUri() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFileName("https://website.com/file.jpg"));

        }
        [TestMethod]
        public void TestGetFileNameFromUnrootedUri() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFileName("path/file.jpg"));

        }
        [TestMethod]
        public void TestGetFileNameFromUriWithUriParameters() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFileName("path/file.jpg?file=1#anchor"));

        }
        [TestMethod]
        public void TestGetFileNameFromUriWithMultipleUriParameters() {

            // The last URI parameter has its name omitted intentionally (this test is due to a real-world encounter).

            // If this were treated like a regular path, "proxy?img=file=&file.jpg" would be the filename.
            // However, the actual filename here is "proxy", which also does not have a file extension.

            Assert.AreEqual("proxy", PathUtilities.GetFileName("https://website.com/proxy?img=file=&file.jpg"));

        }
        [TestMethod]
        public void TestGetFileNameFromLocalPath() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFileName(@"c:\path\file.jpg"));

        }
        [TestMethod]
        public void TestGetFileNameFromUriWithIllegalCharacters() {

            Assert.AreEqual("|file.jpg", PathUtilities.GetFileName("path/|file.jpg"));

        }
        [TestMethod]
        public void TestGetFileNameFromUriWithIllegalCharactersWithHashCharacter() {

            // The hash character ("#") is a valid path character on Windows.
            // Filenames containing the hash character ("#") are valid on Windows, and the part after the hash should not be stripped.

            Assert.AreEqual("|file.jpg#hash.png", PathUtilities.GetFileName("path/|file.jpg#hash.png"));

        }
        [TestMethod]
        public void TestGetFileNameFromUriWithHashCharacter() {

            // URL fragments begin with the hash ("#") character.
            // Filenames containing the hash character ("#") are valid on Windows, and the part after the hash should not be stripped.

            Assert.AreEqual(@"PathWith#Hash.jpg", PathUtilities.GetFileName(@"C:\PathWith#Hash.jpg"));

        }
        [TestMethod]
        public void TestGetFileNameFromUrlWithHashCharacter() {

            // For URLs, everything after the hash character ("#") should be ignored as it indicates the start of a URI fragment.

            Assert.AreEqual(@"PathWith.png", PathUtilities.GetFileName(@"https://example.com/PathWith.png#Hash.jpg"));

        }
        [TestMethod]
        public void TestGetFileNameWithEmptyString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetFileName(string.Empty));

        }
        [TestMethod]
        public void TestGetFileNameWithNullString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetFileName(null));

        }

        // GetFileNameWithoutExtension

        [TestMethod]
        public void TestGetFileNameWithoutExtension() {

            Assert.AreEqual("file", PathUtilities.GetFileNameWithoutExtension("path/file.jpg"));

        }
        [TestMethod]
        public void TestGetFileNameWithoutExtensionWithParameters() {

            Assert.AreEqual("file", PathUtilities.GetFileNameWithoutExtension("path/file.jpg?file=1#anchor"));

        }
        [TestMethod]
        public void TestGetFileNameWithoutExtensionWithMultiplePeriods() {

            Assert.AreEqual("file.1", PathUtilities.GetFileNameWithoutExtension("path/file.1.jpg"));

        }
        [TestMethod]
        public void TestGetFileNameWithoutExtensionWithNoExtension() {

            Assert.AreEqual("file", PathUtilities.GetFileNameWithoutExtension("path/file"));

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
        [TestMethod]
        public void TestGetFileExtensionWithPathWithoutExtension() {

            Assert.AreEqual(string.Empty, PathUtilities.GetFileExtension(@"c:/file"));

        }
        [TestMethod]
        public void TestGetFileExtensionWithDirectoryPath() {

            Assert.AreEqual(string.Empty, PathUtilities.GetFileExtension(@"c:/folder/"));

        }
        [TestMethod]
        public void TestGetFileExtensionWithEmptyString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetFileExtension(string.Empty));

        }
        [TestMethod]
        public void TestGetFileExtensionWithNullString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetFileExtension(null));

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
        [TestMethod]
        public void TestSetFileExtensionWithEmptyFileExtension() {

            Assert.AreEqual("path/file", PathUtilities.SetFileExtension("path/file.tmp", string.Empty));

        }
        [TestMethod]
        public void TestSetFileExtensionWithNullFileExtension() {

            Assert.AreEqual("path/file", PathUtilities.SetFileExtension("path/file.tmp", null));

        }
        [TestMethod]
        public void TestSetFileExtensionWithPathWithoutFileExtension() {

            Assert.AreEqual("path/file.jpg", PathUtilities.SetFileExtension("path/file", ".jpg"));

        }
        [TestMethod]
        public void TestSetFileExtensionWithFileExtensionWithoutLeadingDot() {

            Assert.AreEqual("path/file.jpg", PathUtilities.SetFileExtension("path/file", "jpg"));

        }

        // GetTemporaryDirectoryPath

        [TestMethod]
        public void TestGetTemporaryDirectoryPathWithEnsureUniqueEnabledCreatesDirectory() {

            // The temporary directory path will be created when "ensure unique" is enabled.

            string temporaryDirectoryPath = PathUtilities.GetTemporaryDirectoryPath(new TemporaryPathOptions() {
                EnsureUnique = true,
            });

            Assert.IsTrue(Directory.Exists(temporaryDirectoryPath));

            Directory.Delete(temporaryDirectoryPath);

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
        public void TestAreEqualWithEqualPathsWithAndWithoutEndingDirectorySeparator() {

            // On both Windows and Unix-based systems, trailing directory separators are irrelevant.
            // Trailing directory separators are typically used to denote directory paths, but we cannot have a directory and file with the same name.
            // For this reason, the two paths must be equivalent to each other.
            // https://unix.stackexchange.com/q/22447

            Assert.IsTrue(PathUtilities.AreEqual(@"C:/path/to/directory", @"C:/path/to/directory/"));

        }
        [TestMethod]
        public void TestAreEqualWithEqualPathsWithOnlySlashes() {

            // On Windows systems, "/" and "\" refer to the same path (the root of the C: drive).
            // On Unix systems, these paths should not be considered equal because only "/" is used for directories.

            Assert.IsTrue(PathUtilities.AreEqual(@"/", @"\"));

        }
        [TestMethod]
        public void TestAreEqualWithEqualPathsWithMixedCase() {

            Assert.IsTrue(PathUtilities.AreEqual(@"C:\path\TO\directory\", @"C:\path\to\DIRECTORY\"));

        }
        [TestMethod]
        public void TestAreEqualWithEqualUrls() {

            Assert.IsTrue(PathUtilities.AreEqual(@"https://stackoverflow.com/questions/", @"https://stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestAreEqualWithUnequalPaths() {

            Assert.IsFalse(PathUtilities.AreEqual(@"C:\path\to\", @"C:\path\to\directory\"));

        }
        [TestMethod]
        public void TestAreEqualWithUnequalPathsWithRepeatingSlashes() {

            // Paths with repeating slashes do not refer to the same directory on any platform.
            // Some web servers will treat repeating slashes as equivalent to a single slash, but this is not a rule.

            Assert.IsFalse(PathUtilities.AreEqual(@"C:/path/to/directory", @"C:/path/to//directory"));

        }
        [TestMethod]
        public void TestAreEqualWithUnequalUrlsWithAndWithoutEndingDirectorySeparator() {

            // URLs with trailing slashes are not treated the same as local paths with trailing slashes.
            // It's up to the web server to decide how to interpret them, and there are certainly cases where the two paths are not equivalent to each other.
            // https://stackoverflow.com/q/942751

            Assert.IsFalse(PathUtilities.AreEqual(@"https://stackoverflow.com/questions", @"https://stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestAreEqualWithBothPathsEmpty() {

            Assert.IsTrue(PathUtilities.AreEqual(string.Empty, string.Empty));

        }
        [TestMethod]
        public void TestAreEqualWithBothPathsNull() {

            Assert.IsTrue(PathUtilities.AreEqual(null, null));

        }
        [TestMethod]
        public void TestAreEqualWithFirstPathEmpty() {

            Assert.IsFalse(PathUtilities.AreEqual(string.Empty, @"C:\path\to\directory\"));

        }
        [TestMethod]
        public void TestAreEqualWithSecondPathEmpty() {

            Assert.IsFalse(PathUtilities.AreEqual(@"C:\path\to\directory\", string.Empty));

        }

        // AreEquivalent

        [TestMethod]
        public void TestAreAreEquivalentWithEquivalentPathsWithDotSeparators() {

            Assert.IsTrue(PathUtilities.AreEquivalent(@"C:\path\to\..\to\directory\", @"C:\path\to\.\directory\"));

        }
        [TestMethod]
        public void TestAreAreEquivalentWithEquivalentUrlsWithDotSeparators() {

            Assert.IsTrue(PathUtilities.AreEquivalent(@"https://unix.stackexchange.com/questions/../questions/", @"https://unix.stackexchange.com/questions/./"));

        }
        [TestMethod]
        public void TestAreEquivalentWithEqualUrlsWithRepeatedSlashesAfterProtocol() {

            // Extra slashes after the scheme are stripped by major web browsers and clients, so the two paths should point to the same location.
            // This isn't technically correct, but is common enough behavior that most clients implement it.
            // https://github.com/curl/curl/issues/791

            Assert.IsTrue(PathUtilities.AreEquivalent(@"https://///stackoverflow.com/questions/", @"https://stackoverflow.com/questions/"));

        }
        [TestMethod]
        public void TestAreAreEquivalentWithUnequivalentPathsWithDotSeparators() {

            Assert.IsFalse(PathUtilities.AreEquivalent(@"C:\path\to\..\to\..\directory\", @"C:\path\to\.\directory\"));

        }
        [TestMethod]
        public void TestAreAreEquivalentWithUnequivalentUrlsWithDotSeparators() {

            Assert.IsFalse(PathUtilities.AreEquivalent(@"https://unix.stackexchange.com/questions/../questions/../", @"https://unix.stackexchange.com/questions/./"));

        }

    }

}