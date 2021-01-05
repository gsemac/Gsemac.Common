using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Core.Tests {

    [TestClass]
    public class VersionTests {

        // Parse

        [TestMethod]
        public void TestParseMSVersion() {

            Assert.AreEqual(new MSVersion(1, 2, 3, 4), Version.Parse("1.2.3.4"));

        }
        [TestMethod]
        public void TestParseSemVersion() {

            Assert.AreEqual(new SemVersion("1.2.3-alpha+abc123"), Version.Parse("1.2.3-alpha+abc123"));

        }
        [TestMethod]
        public void TestParseVersionWithPrefix() {

            Assert.AreEqual(new SemVersion(1, 2, 0), Version.Parse("v1.2.0"));
            Assert.AreEqual(new SemVersion(1, 2, 0), Version.Parse("version 1.2.0"));

        }

    }

}