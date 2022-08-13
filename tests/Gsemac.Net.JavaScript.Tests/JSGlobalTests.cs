using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.JavaScript.Tests {

    [TestClass]
    public class JSGlobalTests {

        // DecodeUri

        [TestMethod]
        public void TestDecodeUri() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/decodeURI

            Assert.AreEqual("https://developer.mozilla.org/ru/docs/JavaScript_шеллы",
                JSGlobal.DecodeUri("https://developer.mozilla.org/ru/docs/JavaScript_%D1%88%D0%B5%D0%BB%D0%BB%D1%8B"));

        }

        // EncodeUri

        [TestMethod]
        public void TestEncodeUriWithReservedCharacters() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual(";,/?:@&=+$#", JSGlobal.EncodeUri(";,/?:@&=+$#"));

        }
        [TestMethod]
        public void TestEncodeUriWithUnreservedCharacters() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("-_.!~*'()", JSGlobal.EncodeUri("-_.!~*'()"));

        }
        [TestMethod]
        public void TestEncodeUriWithAlphanumericCharactersAndWhitespace() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("ABC%20abc%20123", JSGlobal.EncodeUri("ABC abc 123"));

        }

        // EncodeUriComponent

        [TestMethod]
        public void TestEncodeUriComponentWithReservedCharacters() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("%3B%2C%2F%3F%3A%40%26%3D%2B%24%23", JSGlobal.EncodeUriComponent(";,/?:@&=+$#"));

        }
        [TestMethod]
        public void TestEncodeUriComponentWithUnreservedCharacters() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("-_.!~*'()", JSGlobal.EncodeUriComponent("-_.!~*'()"));

        }
        [TestMethod]
        public void TestEncodeUriComponentWithAlphanumericCharactersAndWhitespace() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/encodeURI

            Assert.AreEqual("ABC%20abc%20123", JSGlobal.EncodeUriComponent("ABC abc 123"));

        }

        // IsNaN

        [TestMethod]
        public void TestIsNaNWithNull() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/isNaN

            Assert.IsFalse(JSGlobal.IsNaN(null));

        }
        [TestMethod]
        public void TestIsNaNWithInteger() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/isNaN

            Assert.IsFalse(JSGlobal.IsNaN(37));

        }
        [TestMethod]
        public void TestIsNaNWithIntegerString() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/isNaN

            Assert.IsFalse(JSGlobal.IsNaN("37"));

        }
        [TestMethod]
        public void TestIsNaNWithNumberWithFloatString() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/isNaN

            Assert.IsFalse(JSGlobal.IsNaN("37.37"));

        }
        [TestMethod]
        public void TestIsNaNWithNumberWithFloatWithAlternativeSeparatorString() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/isNaN

            Assert.IsTrue(JSGlobal.IsNaN("37,37"));

        }
        [TestMethod]
        public void TestIsNaNWithNonNumericString() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/isNaN

            Assert.IsTrue(JSGlobal.IsNaN("123ABC"));

        }
        [TestMethod]
        public void TestIsNaNWithEmptyString() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/isNaN

            Assert.IsFalse(JSGlobal.IsNaN(" "));

        }
        [TestMethod]
        public void TestIsNaNWithWhiteSpaceString() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/isNaN

            Assert.IsFalse(JSGlobal.IsNaN("  "));

        }

        // ParseInt

        [TestMethod]
        public void TestParseIntWithBase10() {

            Assert.AreEqual(123, JSGlobal.ParseInt("123", 10));

        }
        [TestMethod]
        public void TestParseIntWithBase16() {

            Assert.AreEqual(291, JSGlobal.ParseInt("0x123"));

        }
        [TestMethod]
        public void TestParseIntWithLeadingZero() {

            // ECMAScript 5 forbids a leading 0 denoting an octal number, and it should be interpreted as base-10.
            // All major browsers follow this convention.

            Assert.AreEqual(123, JSGlobal.ParseInt("0123"));

        }

        // Unescape

        [TestMethod]
        public void TestUnescapeWithAsciiString() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/unescape

            Assert.AreEqual("abc123", JSGlobal.Unescape("abc123"));

        }
        [TestMethod]
        public void TestUnescapeWithUriEncoding() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/unescape

            Assert.AreEqual("äöü", JSGlobal.Unescape("%E4%F6%FC"));

        }
        [TestMethod]
        public void TestUnescapeWithUnicodeUriEncoding() {

            // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/unescape

            Assert.AreEqual("ć", JSGlobal.Unescape("%u0107"));

        }

    }

}