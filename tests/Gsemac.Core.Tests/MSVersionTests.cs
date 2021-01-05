using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Gsemac.Core.Tests {

    [TestClass]
    public class MSVersionTests {

        // Parse

        [TestMethod]
        public void TestParseMsVersionWithMajorMinor() {

            Assert.AreEqual(new MSVersion(1, 2), MSVersion.Parse("1.2"));

        }
        [TestMethod]
        public void TestParseMsVersionWithMajorMinorBuild() {

            Assert.AreEqual(new MSVersion(1, 2, 3), MSVersion.Parse("1.2.3"));

        }
        [TestMethod]
        public void TestParseMsVersionWithMajorMinorBuildRevision() {

            Assert.AreEqual(new MSVersion(1, 2, 3, 4), MSVersion.Parse("1.2.3.4"));

        }

        [TestMethod]
        public void TestParseMsVersionWithMajorAndStrict() {

            Assert.ThrowsException<FormatException>(() => MSVersion.Parse("1"));

        }
        [TestMethod]
        public void TestParseMsVersionWithMajorAndNotStrict() {

            Assert.AreEqual("1", MSVersion.Parse("1", strict: false).ToString());

        }
        [TestMethod]
        public void TestParseMsVersionWithMoreThanFourNumbersAndStrict() {

            Assert.ThrowsException<FormatException>(() => MSVersion.Parse("1.2.3.4.5"));

        }
        [TestMethod]
        public void TestParseMsVersionWithMoreThanFourNumbersAndNotStrict() {

            Assert.AreEqual("1.2.3.4.5", MSVersion.Parse("1.2.3.4.5", strict: false).ToString());

        }

        [TestMethod]
        public void TestParseMsVersionThrowsWithTooFewNumbers() {

            Assert.ThrowsException<FormatException>(() => MSVersion.Parse("1"));

        }
        [TestMethod]
        public void TestParseMsVersionThrowsWithTooManyNumbers() {

            Assert.ThrowsException<FormatException>(() => MSVersion.Parse("1.2.3.4.5"));

        }

        // Other

        [TestMethod]
        public void TestComparison() {

            MSVersion[] versions = new MSVersion[] {
                new MSVersion("1.0.0.1"),
                new MSVersion("1.2"),
                new MSVersion("1.2.3"),
                new MSVersion("0.1"),
            };

            CollectionAssert.AreEqual(new MSVersion[] {
                new MSVersion("0.1"),
                new MSVersion("1.0.0.1"),
                new MSVersion("1.2"),
                new MSVersion("1.2.3"),
            }, versions.OrderBy(v => v).ToArray());

        }

    }

}