using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.Http.Tests {

    [TestClass]
    public class RefreshHeaderTests {

        // Parse

        [TestMethod]
        public void TestParseWithValidRefreshHeaderWithTimeout() {

            RefreshHeader header = RefreshHeader.Parse("Refresh: 5");

            Assert.AreEqual(5, (int)header.Timeout.TotalSeconds);
            Assert.AreEqual(string.Empty, header.Url);

        }
        [TestMethod]
        public void TestParseWithValidRefreshHeaderWithSemicolonAndUrl() {

            RefreshHeader header = RefreshHeader.Parse("Refresh: 5; url=https://www.example.com/");

            Assert.AreEqual(5, (int)header.Timeout.TotalSeconds);
            Assert.AreEqual("https://www.example.com/", header.Url);

        }
        [TestMethod]
        public void TestParseWithValidRefreshHeaderWithCommaAndUrl() {

            RefreshHeader header = RefreshHeader.Parse("Refresh: 3,https://www.example.com/");

            Assert.AreEqual(3, (int)header.Timeout.TotalSeconds);
            Assert.AreEqual("https://www.example.com/", header.Url);

        }
        [TestMethod]
        public void TestParseWithInvalidRefreshHeaderWithNegativeTimeout() {

            // Popular web browsers (e.g. Google Chrome) will ignore redirect headers with negative timeouts, so they should be considered invalid.

            Assert.IsFalse(RefreshHeader.TryParse("Refresh: -1", out _));

        }

        // ToString

        [TestMethod]
        public void TestToStringWithRefreshHeaderWithUrl() {

            RefreshHeader header = RefreshHeader.Parse("refresh: 5; url=https://www.example.com/");

            Assert.AreEqual("refresh: 5; url=https://www.example.com/", header.ToString(), ignoreCase: true);

        }
        [TestMethod]
        public void TestToStringWithRefreshHeaderWithoutUrl() {

            RefreshHeader header = RefreshHeader.Parse("refresh: 5;");

            Assert.AreEqual("refresh: 5", header.ToString(), ignoreCase: true);

        }

    }

}