using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Gsemac.Core.Tests {

    [TestClass]
    public class SemVersionTests {

        // Parse

        [TestMethod]
        public void TestParseSemVersionWithMajorMinorPatch() {

            Assert.AreEqual(new SemVersion(1, 2, 3), SemVersion.Parse("1.2.3"));

        }
        [TestMethod]
        public void TestParseSemVersionWithMajorMinorPatchPreRelease() {

            Assert.AreEqual("1.2.3-alpha", SemVersion.Parse("1.2.3-alpha").ToString());

        }
        [TestMethod]
        public void TestParseSemVersionWithMajorMinorPatchPreReleaseBuild() {

            Assert.AreEqual("1.2.3-alpha+abc123", SemVersion.Parse("1.2.3-alpha+abc123").ToString());

        }
        [TestMethod]
        public void TestParseSemVersionThrowsWithTooFewNumbers() {

            Assert.ThrowsException<FormatException>(() => SemVersion.Parse("1"));
            Assert.ThrowsException<FormatException>(() => SemVersion.Parse("1.2"));

        }
        [TestMethod]
        public void TestParseSemVersionThrowsWithTooManyNumbers() {

            Assert.ThrowsException<FormatException>(() => SemVersion.Parse("1.2.3.4"));

        }

        // Other

        [TestMethod]
        public void TestComparison() {

            SemVersion[] versions = new SemVersion[] {
                new SemVersion("1.4.4"),
                new SemVersion("1.2.0"),
                new SemVersion("1.2.0-alpha"),
                new SemVersion("0.1.0"),
            };

            CollectionAssert.AreEqual(new SemVersion[] {
                new SemVersion("0.1.0"),
                new SemVersion("1.2.0-alpha"),
                new SemVersion("1.2.0"),
                new SemVersion("1.4.4"),
            }, versions.OrderBy(v => v).ToArray());

        }

    }

}