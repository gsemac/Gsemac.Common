using Gsemac.Collections.Extensions;
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
        public void TestParseSemVersionWithMajorMinorPatchBuild() {

            Assert.AreEqual("1.2.3+abc123", SemVersion.Parse("1.2.3+abc123").ToString());

        }

        [TestMethod]
        public void TestParseSemVersionWithMajorAndStrict() {

            Assert.ThrowsException<FormatException>(() => SemVersion.Parse("1"));

        }
        [TestMethod]
        public void TestParseSemVersionWithMajorAndNotStrict() {

            Assert.AreEqual("1", SemVersion.Parse("1", strict: false).ToString());

        }
        [TestMethod]
        public void TestParseSemVersionWithMoreThanThreeNumbersAndStrict() {

            Assert.ThrowsException<FormatException>(() => SemVersion.Parse("1.2.3.4"));

        }
        [TestMethod]
        public void TestParseSemVersionWithMoreThanThreeNumbersAndNotStrict() {

            Assert.AreEqual("1.2.3.4", SemVersion.Parse("1.2.3.4", strict: false).ToString());

        }
        [TestMethod]
        public void TestParseSemVersionWithMoreThanThreeNumbersAndPreReleaseAndNotStrict() {

            Assert.AreEqual("1.2.3.4-alpha", SemVersion.Parse("1.2.3.4-alpha", strict: false).ToString());

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
        public void TestComparisonWithDifferentVersionNumbers() {

            SemVersion[] sortedVersions = new SemVersion[] {
                new SemVersion("0.1.0"),
                new SemVersion("1.2.0-alpha"),
                new SemVersion("1.2.0"),
                new SemVersion("1.4.4"),
            };

            CollectionAssert.AreEqual(sortedVersions, sortedVersions.Shuffle().OrderBy(v => v).ToArray());

        }
        [TestMethod]
        public void TestComparisonWithDifferentPreReleases() {

            SemVersion[] sortedVersions = new SemVersion[] {
                new SemVersion("1.0.0-alpha"),
                new SemVersion("1.0.0-alpha.1"),
                new SemVersion("1.0.0-alpha.beta"),
                new SemVersion("1.0.0-beta"),
                new SemVersion("1.0.0-beta.2"),
                new SemVersion("1.0.0-beta.11"),
                new SemVersion("1.0.0-rc.1"),
                new SemVersion("1.0.0"),
            };

            CollectionAssert.AreEqual(sortedVersions, sortedVersions.Shuffle().OrderBy(v => v).ToArray());

        }

    }

}