using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Utilities.Tests {

    [TestClass]
    public class PathUtilitiesTests {

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
        public void GetPathRelativeToRootWithRoot() {

            Assert.AreEqual(@"Users\Username", PathUtilities.GetRelativePathToRoot(@"\Users\Username"));

        }
        [TestMethod]
        public void GetPathRelativeToRootWithDriveRoot() {

            Assert.AreEqual(@"Users\Username", PathUtilities.GetRelativePathToRoot(@"C:\Users\Username"));

        }
        [TestMethod]
        public void GetPathRelativeToRootWithNetworkShare() {

            Assert.AreEqual(@"Username", PathUtilities.GetRelativePathToRoot(@"\\Share\Users\Username"));

        }
        [TestMethod]
        public void GetPathRelativeToRootWithRelativePath() {

            Assert.AreEqual(@"Users\Username", PathUtilities.GetRelativePathToRoot(@"Users\Username"));

        }
        [TestMethod]
        public void GetPathRelativeToRootWithEmptyString() {

            Assert.AreEqual(string.Empty, PathUtilities.GetRelativePathToRoot(string.Empty));

        }

        // GetFileName

        [TestMethod]
        public void TestGetFilenameFromUri() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFileName("https://website.com/file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromUnrootedUri() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFileName("path/file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromUriWithUriParameters() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFileName("path/file.jpg?file=1#anchor"));

        }
        [TestMethod]
        public void TestGetFilenameFromUriWithMultipleUriParameters() {

            // The last URI parameter has its name omitted intentionally (this test is due to a real-world encounter).

            // If this were treated like a regular path, "proxy?img=file=&file.jpg" would be the filename.
            // However, the actual filename here is "proxy", which also does not have a file extension.

            Assert.AreEqual("proxy", PathUtilities.GetFileName("https://website.com/proxy?img=file=&file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromLocalPath() {

            Assert.AreEqual("file.jpg", PathUtilities.GetFileName(@"c:\path\file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameFromUriWithIllegalCharacters() {

            Assert.AreEqual("|file.jpg", PathUtilities.GetFileName("path/|file.jpg"));

        }

        // GetFileNameWithoutExtension

        [TestMethod]
        public void TestGetFilenameWithoutExtension() {

            Assert.AreEqual("file", PathUtilities.GetFileNameWithoutExtension("path/file.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameWithoutExtensionWithParameters() {

            Assert.AreEqual("file", PathUtilities.GetFileNameWithoutExtension("path/file.jpg?file=1#anchor"));

        }
        [TestMethod]
        public void TestGetFilenameWithoutExtensionWithMultiplePeriods() {

            Assert.AreEqual("file.1", PathUtilities.GetFileNameWithoutExtension("path/file.1.jpg"));

        }
        [TestMethod]
        public void TestGetFilenameWithoutExtensionWithNoExtension() {

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

            Assert.IsTrue(PathUtilities.IsLocalPath(@"\user\documents\", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithRelativeDirectoryPath() {

            Assert.IsTrue(PathUtilities.IsLocalPath(@"documents\", verifyPathExists: false));

        }
        [TestMethod]
        public void TestIsLocalPathWithAbsoluteDirectoryPathWithInvalidCharacters() {

            // IsLocalPath does not attempt to do any validation.

            Assert.IsTrue(PathUtilities.IsLocalPath(@"documents|\", verifyPathExists: false));

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

    }

}