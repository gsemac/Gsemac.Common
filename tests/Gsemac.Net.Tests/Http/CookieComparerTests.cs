using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Net.Http.Tests {

    [TestClass]
    public class CookieComparerTests {

        // Compare

        [TestMethod]
        public void TestCompareNullCookieComesBeforeNonNullCookie() {

            Cookie x = null;
            Cookie y = new("name", "value", "/", "example.com");

            Assert.IsLessThan(0, new CookieComparer().Compare(x, y));
            Assert.IsGreaterThan(0, new CookieComparer().Compare(y, x));

        }
        [TestMethod]
        public void TestCompareLongerPathComesBeforeShorterPath() {

            Cookie x = new("name", "value", "/zzz/", "example.com");
            Cookie y = new("name", "value", "/a/", "example.com");

            Assert.IsLessThan(0, new CookieComparer().Compare(x, y));
            Assert.IsGreaterThan(0, new CookieComparer().Compare(y, x));

        }

        // Equals

        [TestMethod]
        public void TestEqualsWithIdenticalCookies() {

            Cookie x = new("name", "value", "/", "example.com");
            Cookie y = new("name", "value", "/", "example.com");

            Assert.IsTrue(new CookieComparer().Equals(x, y));

        }
        [TestMethod]
        public void TestEqualsWithCookiesWithDifferentNames() {

            Cookie x = new("name1", "value", "/", "example.com");
            Cookie y = new("name2", "value", "/", "example.com");

            Assert.IsFalse(new CookieComparer().Equals(x, y));

        }
        [TestMethod]
        public void TestEqualsWithCookiesWithDifferentValues() {

            // Values don't matter when comparing cookies.

            Cookie x = new("name", "value1", "/", "example.com");
            Cookie y = new("name", "value2", "/", "example.com");

            Assert.IsTrue(new CookieComparer().Equals(x, y));

        }
        [TestMethod]
        public void TestEqualsWithCookiesWithDifferentPaths() {

            Cookie x = new("name", "value", "/test/", "example.com");
            Cookie y = new("name", "value", "/", "example.com");

            Assert.IsFalse(new CookieComparer().Equals(x, y));

        }
        [TestMethod]
        public void TestEqualsWithCookiesWithDifferentDomains() {

            Cookie x = new("name", "value", "/", "example.com");
            Cookie y = new("name", "value", "/", "other.com");

            Assert.IsFalse(new CookieComparer().Equals(x, y));

        }
        [TestMethod]
        public void TestEqualsWithCookiesWithEqualDomainsWithDot() {

            Cookie x = new("name", "value", "/", ".example.com");
            Cookie y = new("name", "value", "/", "example.com");

            Assert.IsTrue(new CookieComparer().Equals(x, y));
            Assert.IsTrue(new CookieComparer().Equals(y, x));

        }
        [TestMethod]
        public void TestEqualsWithCookiesWithNamesInDifferentCase() {

            // Name comparisons are case-sensitive.

            Cookie x = new("name", "value", "/", "example.com");
            Cookie y = new("Name", "value", "/", "other.com");

            Assert.IsFalse(new CookieComparer().Equals(x, y));

        }
        [TestMethod]
        public void TestEqualsWithCookiesWithPathsInDifferentCase() {

            // Path comparisons are case-sensitive.

            Cookie x = new("name", "value", "/test/", "example.com");
            Cookie y = new("Name", "value", "/TEST/", "example.com");

            Assert.IsFalse(new CookieComparer().Equals(x, y));

        }
        [TestMethod]
        public void TestEqualsWithCookiesWithDomainsInDifferentCase() {

            // Domain comparisons are not case-sensitive.

            Cookie x = new("name", "value", "/", "example.com");
            Cookie y = new("name", "value", "/", "EXAMPLE.COM");

            Assert.IsTrue(new CookieComparer().Equals(x, y));

        }

    }

}