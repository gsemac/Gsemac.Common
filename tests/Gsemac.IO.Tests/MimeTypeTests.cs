using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class MimeTypeTests {

        // Parse

        [TestMethod]
        public void TestParameterNamesAreCaseInsensitive() {

            IMimeType mimeType = MimeType.Parse("text/plain;CHARSET=US-ASCII");

            Assert.AreEqual("US-ASCII", mimeType.Parameters["charset"]);

        }
        [TestMethod]
        public void TestParsePreservesParameterValueCasing() {

            // The type, subtype, and parameter names should be case-insensitive, but parameter value casing should be preserved (RFC 2045).
            // https://stackoverflow.com/a/12869287/5383169 (Trott)

            IMimeType mimeType = MimeType.Parse("text/plain;charset=US-ASCII");

            Assert.AreEqual("US-ASCII", mimeType.Parameters["charset"]);

        }
        [TestMethod]
        public void TestParseWithMultipleParameters() {

            // This example data is sourced from https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax

            IMimeType mimeType = MimeType.Parse("text/plain;charset=UTF-8;page=21");

            Assert.AreEqual("UTF-8", mimeType.Parameters["charset"]);
            Assert.AreEqual("21", mimeType.Parameters["page"]);

        }

        // ToString

        [TestMethod]
        public void TestToStringPreservesParameterValueCasing() {

            IMimeType mimeType = MimeType.Parse("text/plain;charset=US-ASCII");

            Assert.AreEqual("text/plain;charset=US-ASCII", mimeType.ToString());

        }

    }

}