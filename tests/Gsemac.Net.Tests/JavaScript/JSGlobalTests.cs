using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.JavaScript.Tests {

    [TestClass]
    public class JsGlobalTests {

        [TestMethod]
        public void TestDecodeUri() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/decodeURI

            Assert.AreEqual("https://developer.mozilla.org/ru/docs/JavaScript_шеллы",
                JSGlobal.DecodeUri("https://developer.mozilla.org/ru/docs/JavaScript_%D1%88%D0%B5%D0%BB%D0%BB%D1%8B"));

        }

        [TestMethod]
        public void TestEncodeUriWithReservedCharacters() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual(";,/?:@&=+$#", JSGlobal.EncodeUri(";,/?:@&=+$#"));

        }
        [TestMethod]
        public void TestEncodeUriWithUnreservedCharacters() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("-_.!~*'()", JSGlobal.EncodeUri("-_.!~*'()"));

        }
        [TestMethod]
        public void TestEncodeUriWithAlphanumericCharactersAndWhitespace() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("ABC%20abc%20123", JSGlobal.EncodeUri("ABC abc 123"));

        }

        [TestMethod]
        public void TestEncodeUriComponentWithReservedCharacters() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("%3B%2C%2F%3F%3A%40%26%3D%2B%24%23", JSGlobal.EncodeUriComponent(";,/?:@&=+$#"));

        }
        [TestMethod]
        public void TestEncodeUriComponentWithUnreservedCharacters() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("-_.!~*'()", JSGlobal.EncodeUriComponent("-_.!~*'()"));

        }
        [TestMethod]
        public void TestEncodeUriComponentWithAlphanumericCharactersAndWhitespace() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("ABC%20abc%20123", JSGlobal.EncodeUriComponent("ABC abc 123"));

        }

        [TestMethod]
        public void TestPartIntWithBase10() {

            Assert.AreEqual(123, JSGlobal.ParseInt("123", 10));

        }
        [TestMethod]
        public void TestPartIntWithBase16() {

            Assert.AreEqual(291, JSGlobal.ParseInt("0x123"));

        }
        [TestMethod]
        public void TestPartIntWithLeadingZero() {

            // ECMAScript 5 forbids a leading 0 denoting an octal number, and it should be interpreted as base-10.
            // All major browsers follow this convention.

            Assert.AreEqual(123, JSGlobal.ParseInt("0123"));

        }

        [TestMethod]
        public void TestUnescapeWithAsciiString() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/unescape

            Assert.AreEqual("abc123", JSGlobal.Unescape("abc123"));

        }
        [TestMethod]
        public void TestUnescapeWithUriEncoding() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/unescape

            Assert.AreEqual("äöü", JSGlobal.Unescape("%E4%F6%FC"));

        }
        [TestMethod]
        public void TestUnescapeWithUnicodeUriEncoding() {

            // Example from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/unescape

            Assert.AreEqual("ć", JSGlobal.Unescape("%u0107"));

        }

    }

}