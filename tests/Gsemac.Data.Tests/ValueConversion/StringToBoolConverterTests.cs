using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class StringToBoolConverterTests {

        // Convert

        [TestMethod]
        public void TestConvertWithTrueString() {

            Assert.AreEqual(true, new StringToBoolConverter().Convert("true"));

        }
        [TestMethod]
        public void TestConvertWithFalseString() {

            Assert.AreEqual(false, new StringToBoolConverter().Convert("false"));

        }
        [TestMethod]
        public void TestConvertWithMixedCaseString() {

            Assert.AreEqual(true, new StringToBoolConverter().Convert("tRuE"));
            Assert.AreEqual(false, new StringToBoolConverter().Convert("fAlSe"));

        }

    }

}