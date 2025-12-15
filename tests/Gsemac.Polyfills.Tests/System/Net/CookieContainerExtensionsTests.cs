using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Polyfills.System.Net.Tests {
    [TestClass]
    public class CookieContainerExtensionsTests {
        // GetAllCookies
        [TestMethod]
        public void TestGetAllCookiesReturnsAllCookies() {
            var container = new CookieContainer();
            container.Add(new Cookie("name", "value1", "/", "example.com"));
            container.Add(new Cookie("name", "value2", "/", ".example.com"));

            CookieCollection cookies = CookieContainerExtensions.GetAllCookies(container);

#if NET9_0_OR_GREATER
            // CookieContainer aligns more closely with RFC 6265 from NET 9+.
            // The leading dot is ignored when storing domains in the container.
            Assert.HasCount(1, cookies);
#else
            Assert.HasCount(2, cookies);
#endif
        }
    }
}