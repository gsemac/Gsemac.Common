using Gsemac.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.Tests.Http {

    [TestClass]
    public class HttpHeaderTests {

        // Parse

        [TestMethod]
        public void TestParseHttpHeader() {

            IHttpHeader header = HttpHeader.Parse("Name: Value");

            Assert.AreEqual("Name", header.Name);
            Assert.AreEqual("Value", header.Value);

        }
        [TestMethod]
        public void TestParseHttpHeaderWithWhiteSpaceSurroundingFieldName() {

            // Whitespace should not be trimmed from the field name (ideally, it should not be present at all).
            // https://stackoverflow.com/a/61632443/5383169

            IHttpHeader header = HttpHeader.Parse("  Name  : Value");

            Assert.AreEqual("  Name  ", header.Name);
            Assert.AreEqual("Value", header.Value);

        }
        [TestMethod]
        public void TestParseHttpHeaderWithWhiteSpaceSurroundingFieldValue() {

            // Whitespace should be trimmed around field values.
            // https://stackoverflow.com/a/61632443/5383169

            IHttpHeader header = HttpHeader.Parse("Name:   Value    ");

            Assert.AreEqual("Name", header.Name);
            Assert.AreEqual("Value", header.Value);

        }
        [TestMethod]
        public void TestParseHttpHeaderWithWhitespaceFieldValue() {

            IHttpHeader header = HttpHeader.Parse("Name:       ");

            Assert.AreEqual("Name", header.Name);
            Assert.AreEqual(string.Empty, header.Value);

        }
        [TestMethod]
        public void TestParseHttpHeaderWithFieldValueContainingWhiteSpace() {

            IHttpHeader header = HttpHeader.Parse("Date: Tue, 19 Dec 2023 00:55:08 GMT   ");

            Assert.AreEqual("Date", header.Name);
            Assert.AreEqual("Tue, 19 Dec 2023 00:55:08 GMT", header.Value);

        }
        [TestMethod]
        public void TestParseHttpHeaderWithEmptyFieldValue() {

            // Empty field values should be permitted.
            // https://stackoverflow.com/a/12131993/5383169

            IHttpHeader header = HttpHeader.Parse("Name:");

            Assert.AreEqual(header.Name, "Name");
            Assert.AreEqual(string.Empty, header.Value);

        }
        [TestMethod]
        public void TestParseHttpHeaderWithNoWhiteSpace() {

            IHttpHeader header = HttpHeader.Parse("Name:Value");

            Assert.AreEqual("Name", header.Name);
            Assert.AreEqual("Value", header.Value);

        }

    }

}