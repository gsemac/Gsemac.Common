using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Reflection.Tests {

    [TestClass]
    public class ObjectUtilitiesTests {

        // TryCast

        [TestMethod]
        public void TryCastStringToEnumWithValidName() {

            Assert.IsTrue(ObjectUtilities.TryCast("Item3", out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }
        [TestMethod]
        public void TryCastStringToEnumWithValidCaseInsensitiveName() {

            Assert.IsTrue(ObjectUtilities.TryCast("item3", out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }
        [TestMethod]
        public void TryCastStringToEnumWithValidIntegralString() {

            Assert.IsTrue(ObjectUtilities.TryCast("2", out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }
        [TestMethod]
        public void TryCastIntToEnumWithValidValue() {

            Assert.IsTrue(ObjectUtilities.TryCast(2, out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }

    }

}