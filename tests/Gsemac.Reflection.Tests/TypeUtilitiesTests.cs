using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Reflection.Tests {

    [TestClass]
    public class TypeUtilitiesTests {

        // TryCast

        [TestMethod]
        public void TryCastStringToEnumWithValidName() {

            Assert.IsTrue(TypeUtilities.TryCast("Item3", out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }
        [TestMethod]
        public void TryCastStringToEnumWithValidCaseInsensitiveName() {

            Assert.IsTrue(TypeUtilities.TryCast("item3", out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }
        [TestMethod]
        public void TryCastStringToEnumWithValidIntegralString() {

            Assert.IsTrue(TypeUtilities.TryCast("2", out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }
        [TestMethod]
        public void TryCastIntToEnumWithValidValue() {

            Assert.IsTrue(TypeUtilities.TryCast(2, out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }

    }

}