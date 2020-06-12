using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Utilities.Tests {

    [TestClass]
    public class PathUtilitiesTests {

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