using Gsemac.Net.Http.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Net.Http.Tests {

    [TestClass]
    public class HttpWebRequestOptionsFactoryTests {

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

    }

}