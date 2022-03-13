using Gsemac.Net.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Gsemac.Net.Tests {

    [TestClass]
    public class HttpResponseReaderTests {

        // StatusLine

        [TestMethod]
        public void TestStatusLineReturnsMatchingProperties() {

            using Stream stream = GetHttpResponseStream();
            using IHttpResponseReader reader = new HttpResponseReader(stream);

            Assert.IsNotNull(reader.StartLine);
            Assert.AreEqual(new Version(1, 1), reader.StartLine.ProtocolVersion);
            Assert.AreEqual(HttpStatusCode.OK, reader.StartLine.StatusCode);
            Assert.AreEqual("OK", reader.StartLine.StatusDescription);

        }
        [TestMethod]
        public void TestStatusLineBeforeHeadersReturnsValidStatusLine() {

            using Stream stream = GetHttpResponseStream();
            using IHttpResponseReader reader = new HttpResponseReader(stream);

            Assert.IsNotNull(reader.StartLine);

        }
        [TestMethod]
        public void TestStatusLineAfterHeadersReturnsValidStatusLine() {

            using Stream stream = GetHttpResponseStream();
            using IHttpResponseReader reader = new HttpResponseReader(stream);

            Assert.IsNotNull(reader.Headers);
            Assert.IsNotNull(reader.StartLine);

        }

        // Headers

        [TestMethod]
        public void TestHeadersBeforeStatusLineReturnsValidHeaders() {

            using Stream stream = GetHttpResponseStream();
            using IHttpResponseReader reader = new HttpResponseReader(stream);

            Assert.IsNotNull(reader.Headers);
            Assert.AreEqual(8, reader.Headers.Count());

        }
        [TestMethod]
        public void TestHeadersAfterStatusLineReturnsValidHeaders() {

            using Stream stream = GetHttpResponseStream();
            using IHttpResponseReader reader = new HttpResponseReader(stream);

            Assert.IsNotNull(reader.StartLine);
            Assert.IsNotNull(reader.Headers);
            Assert.AreEqual(8, reader.Headers.Count());

        }
        [TestMethod]
        public void TestHeadersReturnsExpectedHeaderValues() {

            using Stream stream = GetHttpResponseStream();
            using IHttpResponseReader reader = new HttpResponseReader(stream);

            IEnumerable<IHttpHeader> expectedHeaders = new[] {
                new HttpHeader("Date", "Sun, 10 Oct 2010 23:26:07 GMT"),
                new HttpHeader("Server", "Apache/2.2.8 (Ubuntu) mod_ssl/2.2.8 OpenSSL/0.9.8g"),
                new HttpHeader("Last-Modified", "Sun, 26 Sep 2010 22:04:35 GMT"),
                new HttpHeader("ETag", "\"45b6-834-49130cc1182c0\""),
                new HttpHeader("Accept-Ranges", "bytes"),
                new HttpHeader("Content-Length", "12"),
                new HttpHeader("Connection", "close"),
                new HttpHeader("Content-Type", "text/html"),
            };

            Assert.IsTrue(expectedHeaders
                .Zip(reader.Headers)
                .All(pair => pair.First.Name == pair.Second.Name && pair.First.Value == pair.Second.Value));

        }

        // GetBodyStream

        [TestMethod]
        public void TestReadBodyStreamBeforeHeadersReturnsValidBody() {

            using Stream stream = GetHttpResponseStream();
            using IHttpResponseReader reader = new HttpResponseReader(stream);
            using Stream bodyStream = reader.GetBodyStream();
            using StreamReader streamReader = new(bodyStream);

            Assert.IsNotNull(reader.Headers);
            Assert.AreEqual("Hello world!", streamReader.ReadToEnd());

        }
        [TestMethod]
        public void TestReadBodyStreamAfterHeadersReturnsValidBody() {

            using Stream stream = GetHttpResponseStream();
            using IHttpResponseReader reader = new HttpResponseReader(stream);
            using Stream bodyStream = reader.GetBodyStream();
            using StreamReader streamReader = new(bodyStream);

            Assert.AreEqual("Hello world!", streamReader.ReadToEnd());

        }

        private static Stream GetHttpResponseStream() {

            return File.OpenRead(Path.Combine(SamplePaths.HttpSamplesDirectoryPath, "HttpResponse.txt"));

        }

    }

}