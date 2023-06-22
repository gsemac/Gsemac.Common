using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.Http.Tests {

    [TestClass]
    public class RefreshHeaderTests {

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

    }

}