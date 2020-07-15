using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Utilities.Tests {

    [TestClass]
    public class PathUtilitiesTests {

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

        [TestMethod]
        public void TestAnonymizePathWithUserDirectory() {

            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = System.IO.Path.Combine(userDirectory, "test");
            string anonymizedPath = PathUtilities.AnonymizePath(path);

            Assert.AreEqual(@"%USERPROFILE%\test", anonymizedPath);

        }

    }

}