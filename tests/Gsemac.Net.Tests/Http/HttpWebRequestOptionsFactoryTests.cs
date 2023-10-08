using Gsemac.Net.Http.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Gsemac.Net.Http.Tests {

    [TestClass]
    public class HttpWebRequestOptionsFactoryTests {

        // Create

        [TestMethod]
        public void TestCreateWithoutAddingReturnsDefault() {

            IHttpWebRequestOptions defaultOptions = new HttpWebRequestOptions() {
                UserAgent = "hello world",
            };

            Assert.AreEqual(defaultOptions.UserAgent, new HttpWebRequestOptionsFactory(defaultOptions).Create("https://stackoverflow.com/").UserAgent);

        }
        [TestMethod]
        public void TestCreateAfterAddingWithMatch() {

            HttpWebRequestOptionsFactory webRequestOptionsFactory = new HttpWebRequestOptionsFactory();

            webRequestOptionsFactory.Add(new Uri("https://stackoverflow.com"), new HttpWebRequestOptions() {
                UserAgent = "hello world",
            });

            Assert.AreEqual("hello world", webRequestOptionsFactory.Create("https://stackoverflow.com/questions").UserAgent);

        }
        [TestMethod]
        public void TestCreateAfterAddingWithoutMatch() {

            HttpWebRequestOptionsFactory webRequestOptionsFactory = new HttpWebRequestOptionsFactory();

            webRequestOptionsFactory.Add(new Uri("https://stackoverflow.com/questions"), new HttpWebRequestOptions() {
                UserAgent = "hello world",
            });

            Assert.AreNotEqual("hello world", webRequestOptionsFactory.Create("https://stackoverflow.com/").UserAgent);

        }
        [TestMethod]
        public void TestCreateAfterAddingWithoutMatchWithSimilarUri() {

            // HttpWebRequestOptionsFactory matches URIs to options by checking if the given URI is equal to or nested under the URI associated with the options.
            // URIs with domain names such that one is a substring of the other should not be matched.

            HttpWebRequestOptionsFactory webRequestOptionsFactory = new HttpWebRequestOptionsFactory();

            webRequestOptionsFactory.Add(new Uri("https://stackoverflow.com"), new HttpWebRequestOptions() {
                UserAgent = "hello world",
            });

            Assert.AreNotEqual("hello world", webRequestOptionsFactory.Create("https://stackoverflow.comm").UserAgent);

        }

        [TestMethod]
        public void TestCreateAlwaysReturnsTheLatestDefaultProxy() {

            // The default proxy (WebRequest.DefaultProxy) should not be cached, so we always have the most up-to-date value.
            // HttpWebRequestOptions don't cache the proxy either, and always return the most up-to-date value.

            HttpWebRequestOptionsFactory webRequestOptionsFactory = new HttpWebRequestOptionsFactory();

            IHttpWebRequestOptions options1 = webRequestOptionsFactory.Create();

            IWebProxy webProxy = new WebProxy("http://127.0.0.1", 8888);

            WebRequest.DefaultWebProxy = webProxy;

            IHttpWebRequestOptions options2 = webRequestOptionsFactory.Create();

            Assert.IsTrue(ReferenceEquals(options1.Proxy, webProxy));
            Assert.IsTrue(ReferenceEquals(options2.Proxy, webProxy));

        }

    }

}