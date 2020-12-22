using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.JavaScript.Tests {

    [TestClass]
    public class JsWindowTests {

        [TestMethod]
        public void TestBtoaWithAsciiString() {

            Assert.AreEqual("aGVsbG8gd29ybGQ=", JsWindow.Btoa("hello world"));

        }
        [TestMethod]
        public void TestAtobWithAsciiString() {

            Assert.AreEqual("hello world", JsWindow.Atob("aGVsbG8gd29ybGQ="));

        }

    }

}