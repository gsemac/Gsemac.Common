using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniSectionTests {

        // this[]

        [TestMethod]
        public void TestGetPropertyValueWithInvalidPropertyNameReturnsEmptyString() {

            Assert.AreEqual(string.Empty, new IniSection()[string.Empty]);

        }
        [TestMethod]
        public void TestGetPropertyValueWithNullPropertyNameReturnsEmptyString() {

            Assert.AreEqual(string.Empty, new IniSection()[null]);

        }

    }

}