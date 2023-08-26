using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.Http.Tests {

    [TestClass]
    public class RefreshHeaderValueTests {

        // Parse

        [TestMethod]
        public void TestParseWithValidValueWithTimeout() {

            RefreshHeaderValue header = RefreshHeaderValue.Parse("5");

            Assert.AreEqual(5, (int)header.Timeout.TotalSeconds);
            Assert.AreEqual(string.Empty, header.Url);

        }
        [TestMethod]
        public void TestParseWithValidValueWithSemicolonAndUrl() {

            RefreshHeaderValue header = RefreshHeaderValue.Parse("5; url=https://www.example.com/");

            Assert.AreEqual(5, (int)header.Timeout.TotalSeconds);
            Assert.AreEqual("https://www.example.com/", header.Url);

        }
        [TestMethod]
        public void TestParseWithValidValueWithCommaAndUrl() {

            RefreshHeaderValue header = RefreshHeaderValue.Parse("3,https://www.example.com/");

            Assert.AreEqual(3, (int)header.Timeout.TotalSeconds);
            Assert.AreEqual("https://www.example.com/", header.Url);

        }
        [TestMethod]
        public void TestParseWithValidValueWithHeaderName() {

            RefreshHeaderValue header = RefreshHeaderValue.Parse("3,https://www.example.com/");

            Assert.AreEqual(3, (int)header.Timeout.TotalSeconds);
            Assert.AreEqual("https://www.example.com/", header.Url);

        }
        [TestMethod]
        public void TestParseWithInvalidValueWithNegativeTimeout() {

            // Popular web browsers (e.g. Google Chrome) will ignore redirect headers with negative timeouts, so they should be considered invalid.

            Assert.IsFalse(RefreshHeaderValue.TryParse("-1", out _));

        }

        // ToString

        [TestMethod]
        public void TestToStringWithValueWithUrl() {

            RefreshHeaderValue header = RefreshHeaderValue.Parse("5; url=https://www.example.com/");

            Assert.AreEqual("5; url=https://www.example.com/", header.ToString(), ignoreCase: true);

        }
        [TestMethod]
        public void TestToStringWithValueWithoutUrl() {

            RefreshHeaderValue header = RefreshHeaderValue.Parse("5;");

            Assert.AreEqual("5", header.ToString(), ignoreCase: true);

        }

    }

}