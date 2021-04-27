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

        // Username

        [TestMethod]
        public void TestGetUsername() {

            Assert.AreEqual("username", new Url("https://username:password@stackoverflow.com/").Username);

        }
        [TestMethod]
        public void TestToStringAfterSetUsernameWithoutPassword() {

            IUrl url = new Url("https://stackoverflow.com/") {
                Username = "username",
            };

            Assert.AreEqual("https://username@stackoverflow.com/", url.ToString());

        }
        [TestMethod]
        public void TestToStringAfterSetUsernameWithPassword() {

            IUrl url = new Url("https://stackoverflow.com/") {
                Username = "username",
                Password = "password",
            };

            Assert.AreEqual("https://username:password@stackoverflow.com/", url.ToString());

        }

    }

}