using Gsemac.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Gsemac.Net.Tests {

    [TestClass]
    public class DataUrlTests {

        // Parse

        [TestMethod]
        public void TestParseWithBase64EncodedData() {

            DataUrl dataUrl = DataUrl.Parse("data:text/plain;base64,SGVsbG8sIFdvcmxkIQ==");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsTrue(dataUrl.IsBase64Encoded);
            Assert.AreEqual("Hello, World!", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/plain", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithNoMimeTypeAndNoBase64Encoding() {

            DataUrl dataUrl = DataUrl.Parse("data:,Hello%2C%20World%21");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.IsBase64Encoded);
            Assert.AreEqual("Hello, World!", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/plain;charset=US-ASCII", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithMimeTypeWithMultipleParameters() {

            // This example data is sourced from https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax

            DataUrl dataUrl = DataUrl.Parse("data:text/plain;charset=UTF-8;page=21,the%20data:1234,5678");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.IsBase64Encoded);
            Assert.AreEqual("the data:1234,5678", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/plain;charset=UTF-8;page=21", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithUrlEncodedData() {

            DataUrl dataUrl = DataUrl.Parse("data:text/html,%3Ch1%3EHello%2C%20World%21%3C%2Fh1%3E");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.IsBase64Encoded);
            Assert.AreEqual("<h1>Hello, World!</h1>", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/html", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithNonUrlEncodedData() {

            DataUrl dataUrl = DataUrl.Parse("data:text/html,<script>alert('hi');</script>");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.IsBase64Encoded);
            Assert.AreEqual("<script>alert('hi');</script>", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/html", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithEmptyMimeType() {

            // The MIME type is optional, and if not provided, defaults to "text/plain;charset=US-ASCII".
            // https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax

            DataUrl dataUrl = DataUrl.Parse("data:,%3Ch1%3EHello%2C%20World%21%3C%2Fh1%3E");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.IsBase64Encoded);
            Assert.AreEqual("<h1>Hello, World!</h1>", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/plain;charset=US-ASCII", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithEmptyData() {

            DataUrl dataUrl = DataUrl.Parse("data:text/plain;charset=UTF-8,");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.IsBase64Encoded);
            Assert.AreEqual(string.Empty, StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/plain;charset=UTF-8", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithEmptyMimeTypeAndEmptyData() {

            // The shortest possible valid data URI is "data:,".
            // https://en.wikipedia.org/wiki/Data_URI_scheme#Syntax

            DataUrl dataUrl = DataUrl.Parse("data:,");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.IsBase64Encoded);
            Assert.AreEqual(string.Empty, StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/plain;charset=US-ASCII", dataUrl.MimeType.ToString());

        }

    }

}