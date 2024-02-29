using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Gsemac.Net.Http.Headers.Tests {

    [TestClass]
    public class AcceptEncodingHeaderValueTests {

        // Constructor

        [TestMethod]
        public void TestConstructorWithSingleDecompressionMethod() {

            var value = new AcceptEncodingHeaderValue(DecompressionMethods.GZip);

            Assert.IsTrue(value.DecompressionMethods == DecompressionMethods.GZip);
            Assert.AreEqual("gzip", value.ToString());

        }
        [TestMethod]
        public void TestConstructorWithDecompressionMethods() {

            var value = new AcceptEncodingHeaderValue(DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli);

            Assert.IsTrue(value.DecompressionMethods == (DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli));
            Assert.AreEqual("gzip, deflate, br", value.ToString());

        }

        // TryParse

        [TestMethod]
        public void TestTryParseWithNullString() {

            Assert.IsTrue(AcceptEncodingHeaderValue.TryParse(null, out AcceptEncodingHeaderValue value));
            Assert.IsTrue(value.DecompressionMethods == DecompressionMethods.None);
            Assert.AreEqual(string.Empty, value.ToString());

        }
        [TestMethod]
        public void TestTryParseWithEmptyString() {

            Assert.IsTrue(AcceptEncodingHeaderValue.TryParse(string.Empty, out AcceptEncodingHeaderValue value));
            Assert.IsTrue(value.DecompressionMethods == DecompressionMethods.None);
            Assert.AreEqual(string.Empty, value.ToString());

        }
        [TestMethod]
        public void TestTryParseWithSingleEncoding() {

            Assert.IsTrue(AcceptEncodingHeaderValue.TryParse("gzip", out AcceptEncodingHeaderValue value));
            Assert.IsTrue(value.DecompressionMethods == DecompressionMethods.GZip);
            Assert.AreEqual("gzip", value.ToString());

        }
        [TestMethod]
        public void TestTryParseWithMultipleEncodings() {

            Assert.IsTrue(AcceptEncodingHeaderValue.TryParse("gzip, deflate, br", out AcceptEncodingHeaderValue value));
            Assert.IsTrue(value.DecompressionMethods == (DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli));
            Assert.AreEqual("gzip, deflate, br", value.ToString());

        }
        [TestMethod]
        public void TestTryParseWithIdentity() {

            Assert.IsTrue(AcceptEncodingHeaderValue.TryParse("identity", out AcceptEncodingHeaderValue value));
            Assert.IsTrue(value.DecompressionMethods == DecompressionMethods.None);
            Assert.AreEqual("identity", value.ToString());

        }
        [TestMethod]
        public void TestTryParseWithWildcard() {

            Assert.IsTrue(AcceptEncodingHeaderValue.TryParse("*", out AcceptEncodingHeaderValue value));
            Assert.IsTrue(value.DecompressionMethods == DecompressionMethods.All);
            Assert.AreEqual("*", value.ToString());

        }
        [TestMethod]
        public void TestTryParseWithEncodingsWithQualityValues() {

            // Encodings should be sorted by Q-value when present.

            Assert.IsTrue(AcceptEncodingHeaderValue.TryParse("br;q=1.0, gzip;q=0.8, *;q=0.1", out AcceptEncodingHeaderValue value));
            Assert.IsTrue(value.DecompressionMethods == DecompressionMethods.All);
            Assert.AreEqual("br;q=1.0, gzip;q=0.8, *;q=0.1", value.ToString());

        }

    }

}