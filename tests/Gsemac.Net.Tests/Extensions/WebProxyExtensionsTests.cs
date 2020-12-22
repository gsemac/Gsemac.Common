using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Net.Extensions.Tests {

    [TestClass]
    public class WebProxyExtensionsTests {

        [TestMethod]
        public void TestGetProxyStringWithHttpAddress() {

            IWebProxy proxy = new WebProxy("127.0.0.1");

            Assert.AreEqual("http://127.0.0.1:80", proxy.GetProxyString());

        }
        [TestMethod]
        public void TestGetProxyStringWithHttpAddressWithProtocol() {

            IWebProxy proxy = new WebProxy("http://127.0.0.1");

            Assert.AreEqual("http://127.0.0.1:80", proxy.GetProxyString());

        }
        [TestMethod]
        public void TestGetProxyStringWithSocks5AddressWithProtocol() {

            IWebProxy proxy = new WebProxy("socks5://127.0.0.1");

            Assert.AreEqual("socks5://127.0.0.1:1080", proxy.GetProxyString());

        }
        [TestMethod]
        public void TestGetProxyStringWithHttpAddressWithPort() {

            IWebProxy proxy = new WebProxy("http://127.0.0.1:81");

            Assert.AreEqual("http://127.0.0.1:81", proxy.GetProxyString());

        }
        [TestMethod]
        public void TestGetProxyStringWithHttpsAddress() {

            IWebProxy proxy = new WebProxy("https://127.0.0.1");

            Assert.AreEqual("https://127.0.0.1:443", proxy.GetProxyString());

        }
        [TestMethod]
        public void TestGetProxyStringWithUsernamePassword() {

            IWebProxy proxy = new WebProxy("127.0.0.1") {
                Credentials = new NetworkCredential("username", "password")
            };

            Assert.AreEqual("http://username:password@127.0.0.1:80", proxy.GetProxyString());

        }
        [TestMethod]
        public void TestGetProxyStringWithUsernamePasswordWithReservedCharacters() {

            IWebProxy proxy = new WebProxy("127.0.0.1") {
                Credentials = new NetworkCredential("user:name", "pass:word")
            };

            Assert.AreEqual("http://user%3Aname:pass%3Aword@127.0.0.1:80", proxy.GetProxyString());

        }
        [TestMethod]
        public void TestGetProxyStringWithEmptyProxy() {

            IWebProxy proxy = new WebProxy();

            Assert.AreEqual(string.Empty, proxy.GetProxyString());

        }
        [TestMethod]
        public void TestIsEmptyWithEmptyProxy() {

            IWebProxy proxy = new WebProxy();

            Assert.AreEqual(true, proxy.IsEmpty());

        }
        [TestMethod]
        public void TestIsEmptyWithNonEmptyProxy() {

            IWebProxy proxy = new WebProxy("127.0.0.1:80");

            Assert.AreEqual(false, proxy.IsEmpty());

        }

    }

}