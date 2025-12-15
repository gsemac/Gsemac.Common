using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Http.Tests {
    [TestClass]
    public class CookieContainerUtilitiesTests {
        // RemoveDuplicateCookies
        [TestMethod]
        public void TestRemoveDuplicateCookiesWithDifferentEquivalentDomains() {
            CookieContainer container = new();
            container.Add(new Cookie("name", "value1", "/", "example.com"));
            container.Add(new Cookie("name", "value2", "/", ".example.com"));

            CookieCollection cookies = container.GetCookies(new Uri("https://example.com/"));

#if NET9_0_OR_GREATER
            // CookieContainer aligns more closely with RFC 6265 from NET 9+.
            // The leading dot is ignored when storing domains in the container.
            Assert.HasCount(1, cookies);
#else
            Assert.HasCount(2, cookies);
#endif

            CookieContainerUtilities.RemoveDuplicateCookies(container);

            cookies = container.GetCookies(new Uri("https://example.com/"));

            Assert.HasCount(1, cookies);
            Assert.AreEqual("value2", cookies.First().Value);
        }
    }
}