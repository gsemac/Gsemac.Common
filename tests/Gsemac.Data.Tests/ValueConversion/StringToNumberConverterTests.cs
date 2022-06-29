using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class StringToNumberConverter {

        // Convert

        [TestMethod]
        public void TestConvertWithInteger() {

            Assert.AreEqual(69, new StringToNumberConverter<int>().Convert("69"));

        }
        [TestMethod]
        public void TestConvertWithDouble() {

            Assert.AreEqual(69.420, new StringToNumberConverter<double>().Convert("69.420"));

        }

        [TestMethod]
        public void TestConvertWithTypeMismatchFails() {

            // If the converter was instantiated with an integral template type, floating-point numbers should fail to be parsed.
            // This allows the opportunity for strict parsing where required. If it isn't, just use double.

            Assert.IsFalse(new StringToNumberConverter<int>().TryConvert("69.420", out _));

        }

    }

}