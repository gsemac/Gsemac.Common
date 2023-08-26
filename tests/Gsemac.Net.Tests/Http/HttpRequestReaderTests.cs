using Gsemac.Net.Http.Headers;
using Gsemac.Net.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Net.Http.Tests
{

    [TestClass]
    public class HttpRequestReaderTests {

        // StartLine

        [TestMethod]
        public void TestStartLineReturnsMatchingProperties() {

            using Stream stream = GetHttpRequestStream();
            using IHttpRequestReader reader = new HttpRequestReader(stream);

            Assert.IsNotNull(reader.StartLine);
            Assert.AreEqual("GET", reader.StartLine.Method);
            Assert.AreEqual(new Uri("/hello.htm", UriKind.RelativeOrAbsolute), reader.StartLine.RequestUri);
            Assert.AreEqual(new Version(1, 1), reader.StartLine.ProtocolVersion);

        }

        // Headers

        [TestMethod]
        public void TestHeadersReturnsExpectedHeaderValues() {

            using Stream stream = GetHttpRequestStream();
            using IHttpRequestReader reader = new HttpRequestReader(stream);

            IEnumerable<IHttpHeader> expectedHeaders = new[] {
                new HttpHeader("User-Agent", "Mozilla/4.0 (compatible; MSIE5.01; Windows NT)"),
                new HttpHeader("Host", "www.tutorialspoint.com"),
                new HttpHeader("Accept-Language", "en-us"),
                new HttpHeader("Accept-Encoding", "gzip, deflate"),
                new HttpHeader("Connection", "Keep-Alive"),
            };

            Assert.IsTrue(expectedHeaders
                .Zip(reader.Headers)
                .All(pair => pair.First.Name == pair.Second.Name && pair.First.Value == pair.Second.Value));

        }

        private static Stream GetHttpRequestStream() {

            return File.OpenRead(Path.Combine(SamplePaths.HttpSamplesDirectoryPath, "HttpRequest.txt"));

        }

    }

}
