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

            Assert.IsTrue(dataUrl.Base64Encoded);
            Assert.AreEqual("Hello, World!", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/plain", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithNoMimeTypeAndNoBase64Encoding() {

            DataUrl dataUrl = DataUrl.Parse("data:,Hello%2C%20World%21");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.Base64Encoded);
            Assert.AreEqual("Hello, World!", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/plain;charset=US-ASCII", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithUrlEncodedData() {

            DataUrl dataUrl = DataUrl.Parse("data:text/html,%3Ch1%3EHello%2C%20World%21%3C%2Fh1%3E");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.Base64Encoded);
            Assert.AreEqual("<h1>Hello, World!</h1>", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/html", dataUrl.MimeType.ToString());

        }
        [TestMethod]
        public void TestParseWithNonUrlEncodedData() {

            DataUrl dataUrl = DataUrl.Parse("data:text/html,<script>alert('hi');</script>");

            using Stream dataStream = dataUrl.GetDataStream();

            Assert.IsFalse(dataUrl.Base64Encoded);
            Assert.AreEqual("<script>alert('hi');</script>", StringUtilities.StreamToString(dataStream));
            Assert.AreEqual("text/html", dataUrl.MimeType.ToString());

        }

    }

}