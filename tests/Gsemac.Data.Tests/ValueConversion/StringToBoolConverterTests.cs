using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class StringToBoolConverterTests {

        // Convert

        [TestMethod]
        public void TestConvertWithTrueString() {

            Assert.IsTrue(new StringToBoolConverter().Convert("true"));

        }
        [TestMethod]
        public void TestConvertWithFalseString() {

            Assert.IsFalse(new StringToBoolConverter().Convert("false"));

        }
        [TestMethod]
        public void TestConvertWithMixedCaseString() {

            Assert.IsTrue(new StringToBoolConverter().Convert("tRuE"));
            Assert.IsFalse(new StringToBoolConverter().Convert("fAlSe"));

        }

    }

}