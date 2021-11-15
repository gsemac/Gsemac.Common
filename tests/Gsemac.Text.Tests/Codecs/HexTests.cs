using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Text.Codecs.Tests {

    [TestClass]
    public class HexTests {

        [TestMethod]
        public void TestEncodeString() {

            Assert.AreEqual("48656c6c6f2c20776f726c6421", Hex.EncodeString("Hello, world!"));

        }
        [TestMethod]
        public void TestDecodeString() {

            Assert.AreEqual("Hello, world!", Hex.DecodeString("48656c6c6f2c20776f726c6421"));

        }
        [TestMethod]
        public void TestDecodeUppercaseString() {

            // Hexadecimal is case-insensitive.

            Assert.AreEqual("Hello, world!", Hex.DecodeString("48656C6C6F2C20776F726C6421"));

        }
        [TestMethod]
        public void TestDecodeStringWithOddNumberedLength() {

            Assert.AreEqual("\0Hello, world!\0", Hex.DecodeString("048656c6c6f2c20776f726c642100"));

        }

    }

}