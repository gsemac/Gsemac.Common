using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Net.Tests {

    [TestClass]
    public class UrlTests {

        // Scheme

        [TestMethod]
        public void TestGetScheme() {

            Assert.AreEqual("https:", new Url("https://stackoverflow.com/").Scheme);

        }
        [TestMethod]
        public void TestToStringAfterSetSchemeWithColon() {

            IUrl url = new Url("https://stackoverflow.com/") {
                Scheme = "http:",
            };

            Assert.AreEqual("http://stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetSchemeWithoutColon() {

            IUrl url = new Url("https://stackoverflow.com/") {
                Scheme = "http",
            };

            Assert.AreEqual("http://stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestSetSchemeWithInvalidCharacters() {

            Assert.ThrowsException<ArgumentException>(() => new Url("https://stackoverflow.com/").Scheme = "ht%$tps:");

        }

    }

}