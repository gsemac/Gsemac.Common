using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Reflection.Tests {

    [TestClass]
    public class TypeUtilitiesTests {

        // TryCast

        [TestMethod]
        public void TestTryCastStringToEnumWithValidName() {

            Assert.IsTrue(TypeUtilities.TryCast("Item3", out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }
        [TestMethod]
        public void TestTryCastStringToEnumWithValidCaseInsensitiveName() {

            Assert.IsTrue(TypeUtilities.TryCast("item3", out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }
        [TestMethod]
        public void TestTryCastStringToEnumWithValidIntegralString() {

            Assert.IsTrue(TypeUtilities.TryCast("2", out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }
        [TestMethod]
        public void TestTryCastIntToEnumWithValidValue() {

            Assert.IsTrue(TypeUtilities.TryCast(2, out TestEnum enumValue));
            Assert.AreEqual(TestEnum.Item3, enumValue);

        }

        [TestMethod]
        public void TestTryCastToNullableTypeWithNullValue() {

            Assert.IsTrue(TypeUtilities.TryCast(null, out int? result));
            Assert.IsFalse(result.HasValue);

        }
        [TestMethod]
        public void TestTryCastToNullableTypeWithInvalidValue() {

            Assert.IsFalse(TypeUtilities.TryCast("abc", out int? _));

        }
        [TestMethod]
        public void TestTryCastToNullableTypeWithValidValue() {

            Assert.IsTrue(TypeUtilities.TryCast("35", out int? result));
            Assert.AreEqual(35, result.Value);

        }

        [TestMethod]
        public void TestTryCastToNullableEnumTypeWithNullValue() {

            Assert.IsTrue(TypeUtilities.TryCast(null, out TestEnum? result));
            Assert.IsFalse(result.HasValue);

        }
        [TestMethod]
        public void TestTryCastToNullableEnumTypeWithInvalidValue() {

            Assert.IsFalse(TypeUtilities.TryCast("abc", out TestEnum? _));

        }
        [TestMethod]
        public void TestTryCastToNullableEnumTypeWithValidValue() {

            Assert.IsTrue(TypeUtilities.TryCast("0", out TestEnum? result));
            Assert.AreEqual(TestEnum.Item1, result.Value);

        }

        [TestMethod]
        public void TestTryCastWithInvalidValue() {

            Assert.IsFalse(TypeUtilities.TryCast("abc", out int _));

        }
        [TestMethod]
        public void TestTryCastWithValidValue() {

            Assert.IsTrue(TypeUtilities.TryCast("35", out int result));
            Assert.AreEqual(35, result);

        }

    }

}