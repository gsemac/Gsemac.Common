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
        public void TestAnonymizePathWithUserDirectory() {

            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string userDirectoryParentDirectory = System.IO.Path.GetDirectoryName(userDirectory);
            string path = System.IO.Path.Combine(userDirectory, "test");
            string anonymizedPath = PathUtilities.AnonymizePath(path);

            Assert.AreEqual(System.IO.Path.Join(userDirectoryParentDirectory, "%USERNAME%", "test"), anonymizedPath);

        }

    }

}