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

            Assert.AreEqual(2, cookies.Count);

            CookieContainerUtilities.RemoveDuplicateCookies(container);

            cookies = container.GetCookies(new Uri("https://example.com/"));

            Assert.AreEqual(1, cookies.Count);
            Assert.AreEqual("value2", cookies.First().Value);

        }

    }

}