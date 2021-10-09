using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.Tests {

    [TestClass]
    public class RefreshHeaderTests {

        [TestMethod]
        public void TestParseWithValidRefreshHeaderWithSemicolon() {

            RefreshHeader header = RefreshHeader.Parse("Refresh: 5; url=https://www.example.com/");

            Assert.AreEqual(5, (int)header.Timeout.TotalSeconds);
            Assert.AreEqual("https://www.example.com/", header.Url);

        }
        [TestMethod]
        public void TestParseWithValidRefreshHeaderWithComma() {

            RefreshHeader header = RefreshHeader.Parse("Refresh: 3,https://www.example.com/");

            Assert.AreEqual(3, (int)header.Timeout.TotalSeconds);
            Assert.AreEqual("https://www.example.com/", header.Url);

        }

    }

}