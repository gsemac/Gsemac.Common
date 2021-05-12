using Gsemac.Net.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Net.Tests {

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

    }

}