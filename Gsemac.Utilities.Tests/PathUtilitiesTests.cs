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

        // AnonymizePath

        [TestMethod]
        public void TestAnonymizePathWithUserDirectory() {

            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = System.IO.Path.Combine(userDirectory, "test");
            string anonymizedPath = PathUtilities.AnonymizePath(path);

            Assert.AreEqual(@"%USERPROFILE%\test", anonymizedPath);

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