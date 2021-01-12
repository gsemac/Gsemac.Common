using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Core.Extensions.Tests {

    [TestClass]
    public class VersionExtensionsTests {

        // ToVersion

        [TestMethod]
        public void TestToVersionWith1RevisionNumber() {

            Assert.AreEqual(new System.Version(1, 0), new MSVersion(1, 0).ToVersion());

        }
        [TestMethod]
        public void TestToVersionWith2RevisionNumbers() {

            Assert.AreEqual(new System.Version(1, 2), new MSVersion(1, 2).ToVersion());

        }
        [TestMethod]
        public void TestToVersionWith3RevisionNumbers() {

            Assert.AreEqual(new System.Version(1, 2, 3), new MSVersion(1, 2, 3).ToVersion());

        }
        [TestMethod]
        public void TestToVersionWith4RevisionNumbers() {

            Assert.AreEqual(new System.Version(1, 2, 3, 4), new MSVersion(1, 2, 3, 4).ToVersion());

        }
        [TestMethod]
        public void TestToVersionWithMoreThan4RevisionNumbers() {

            Assert.AreEqual(new System.Version(1, 2, 3, 4), Version.Parse("1.2.3.4.5", strict: false).ToVersion());

        }

    }

}