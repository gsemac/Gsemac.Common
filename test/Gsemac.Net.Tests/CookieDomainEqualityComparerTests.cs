using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Net.Tests {

    [TestClass]
    public class CookieDomainEqualityComparerTests {

        [TestMethod]
        public void TestDomainAndDomainWithSubdomain() {

            Assert.IsTrue(new CookieDomainPattern("example.com").IsMatch("www.example.com"));

        }
        [TestMethod]
        public void TestDomainStartingWithDotAndDomainWithSubdomain() {

            Assert.IsTrue(new CookieDomainPattern(".example.com").IsMatch("www.example.com"));

        }
        [TestMethod]
        public void TestDomainStartingWithDotAndDomainWithoutSubdomain() {

            Assert.IsTrue(new CookieDomainPattern(".example.com").IsMatch("example.com"));

        }
        [TestMethod]
        public void TestDomainWithDifferentDomain() {

            Assert.IsFalse(new CookieDomainPattern(".example.com").IsMatch("anotherexample.com"));

        }
        [TestMethod]
        public void TestDomainWithDifferentSubdomain() {

            Assert.IsFalse(new CookieDomainPattern("www.example.com").IsMatch("www2.example.com"));

        }
        [TestMethod]
        public void TestDomainWithTld() {

            Assert.IsFalse(new CookieDomainPattern("example.com").IsMatch(".com"));

        }
        [TestMethod]
        public void TestTldWithTld() {

            Assert.ThrowsException<ArgumentException>(() => new CookieDomainPattern(".com").IsMatch(".com"));

        }

    }

}