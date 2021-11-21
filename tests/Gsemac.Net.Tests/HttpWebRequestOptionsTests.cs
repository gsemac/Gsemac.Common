using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Tests {

    [TestClass]
    public class HttpWebRequestOptionsTests {

        // UserAgent

        [TestMethod]
        public void TestUserAgentPropertyIsSetAfterAddingUserAgentHeader() {

            IHttpWebRequestOptions options = new HttpWebRequestOptions();

            options.Headers.Set(HttpRequestHeader.UserAgent, "test");

            Assert.AreEqual("test", options.UserAgent);

        }
        [TestMethod]
        public void TestUserAgentHeaderIsSetAfterSettingUserAgentProperty() {

            IHttpWebRequestOptions options = new HttpWebRequestOptions() {
                UserAgent = "test",
            };

            Assert.AreEqual("test", options.Headers[HttpRequestHeader.UserAgent]);

        }
        [TestMethod]
        public void TestUserAgentHeaderIsRemovedAfterClearingUserAgentProperty() {

            IHttpWebRequestOptions options = new HttpWebRequestOptions() {
                UserAgent = string.Empty,
            };

            Assert.IsFalse(options.Headers.Keys.Cast<string>()
                .Any(name => name.Equals("user-agent", StringComparison.OrdinalIgnoreCase)));

        }

    }

}