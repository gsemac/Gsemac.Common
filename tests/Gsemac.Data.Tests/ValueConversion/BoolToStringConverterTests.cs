using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class BoolToStringConverterTests {

        // Convert

        [TestMethod]
        public void TestConvertWithTrue() {

            Assert.AreEqual("true", new BoolToStringConverter().Convert(true));

        }
        [TestMethod]
        public void TestConvertWithFalse() {

            Assert.AreEqual("false", new BoolToStringConverter().Convert(false));

        }

    }

}