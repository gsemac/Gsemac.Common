using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Polyfills.System.Net.Tests {

    [TestClass]
    public class CookieContainerExtensionsTests {

        // GetAllCookies

        [TestMethod]
        public void TestGetAllCookiesReturnsAllCookies() {

            CookieContainer container = new();

            container.Add(new Cookie("name", "value1", "/", "example.com"));
            container.Add(new Cookie("name", "value2", "/", ".example.com"));

            CookieCollection cookies = CookieContainerExtensions.GetAllCookies(container);

            Assert.AreEqual(2, cookies.Count);

        }

    }

}