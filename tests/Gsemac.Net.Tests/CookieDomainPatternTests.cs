using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Net.Tests {

    [TestClass]
    public class CookieDomainPatternTests {

        [TestMethod]
        public void TestIsMatchWithDomainPatternAndDomainWithSubdomain() {

            Assert.IsTrue(new CookieDomainPattern("example.com").IsMatch("www.example.com"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternAndDomainWithSubdomainAndPath() {

            Assert.IsTrue(new CookieDomainPattern("example.com").IsMatch("www.example.com/some/path"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternAndUrl() {

            Assert.IsTrue(new CookieDomainPattern("example.com").IsMatch("https://example.com/"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternAndUrlWithSubdomain() {

            Assert.IsTrue(new CookieDomainPattern("example.com").IsMatch("https://www.example.com/"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternAndUrlWithSubdomainAndPath() {

            Assert.IsTrue(new CookieDomainPattern("example.com").IsMatch("https://www.example.com/some/path"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternWithPathAndUrlWithSamePath() {

            Assert.IsTrue(new CookieDomainPattern("example.com/path").IsMatch("https://example.com/path"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternWithPathAndUrlWithSubpath() {

            // Subpaths should have access to cookies set for their superpaths, but NOT the other way around.
            // https://stackoverflow.com/a/576561/5383169

            Assert.IsTrue(new CookieDomainPattern("example.com/path").IsMatch("https://example.com/path/path2"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternWithPathAndUrlWithSuperpath() {

            // Subpaths should have access to cookies set for their superpaths, but NOT the other way around.
            // https://stackoverflow.com/a/576561/5383169

            Assert.IsFalse(new CookieDomainPattern("example.com/path/path2").IsMatch("https://example.com/path/"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternWithPathAndUrlWithSamePathAndQueryParameters() {

            Assert.IsTrue(new CookieDomainPattern("example.com/path").IsMatch("https://example.com/path?name=value"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternWithPathAndUrlWithSamePathAndFragment() {

            Assert.IsTrue(new CookieDomainPattern("example.com/path").IsMatch("https://example.com/path#fragment"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternWithPathAndUrlWithDifferentPath() {

            Assert.IsFalse(new CookieDomainPattern("example.com/path").IsMatch("https://example.com/different"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternStartingWithDotAndDomainWithSubdomain() {

            Assert.IsTrue(new CookieDomainPattern(".example.com").IsMatch("www.example.com"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternStartingWithDotAndDomainWithoutSubdomain() {

            Assert.IsTrue(new CookieDomainPattern(".example.com").IsMatch("example.com"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternWithDifferentDomain() {

            Assert.IsFalse(new CookieDomainPattern(".example.com").IsMatch("anotherexample.com"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternWithDifferentSubdomain() {

            Assert.IsFalse(new CookieDomainPattern("www.example.com").IsMatch("www2.example.com"));

        }
        [TestMethod]
        public void TestIsMatchWithDomainPatternAndTld() {

            Assert.IsFalse(new CookieDomainPattern("example.com").IsMatch(".com"));

        }
        [TestMethod]
        public void TestIsMatchWithTldPatternAndTld() {

            Assert.ThrowsException<ArgumentException>(() => new CookieDomainPattern(".com").IsMatch(".com"));

        }

    }

}