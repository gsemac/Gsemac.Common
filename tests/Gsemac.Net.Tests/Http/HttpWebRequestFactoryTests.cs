using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Net.Http.Tests {

    [TestClass]
    public class HttpWebRequestFactoryTests {

        // Create

        [TestMethod]
        public void TestRequestsCookieContainerIsNullByDefault() {

            // HttpWebRequest's "CookieContainer" property should be null by default.
            // If the "cookie" header is set manually, it will only be sent if this property is null.

            IHttpWebRequest webRequest = HttpWebRequestFactory.Default.Create("http://127.0.0.1/");

            Assert.AreEqual(null, webRequest.CookieContainer);

        }
        [TestMethod]
        public void TestRequestsProxyIsSetAccordingToHttpWebRequestOptions() {

            // HttpWebRequest's "CookieContainer" property should be null by default.
            // If the "cookie" header is set manually, it will only be sent if this property is null.

            IWebProxy proxy = new WebProxy("127.0.0.1", 8888);

            IHttpWebRequestFactory webRequestFactory = new HttpWebRequestFactory(new HttpWebRequestOptions() {
                Proxy = proxy,
            });

            IHttpWebRequest webRequest = webRequestFactory.Create("http://127.0.0.1/");

            Assert.IsTrue(ReferenceEquals(webRequest.Proxy, proxy));

        }

    }

}