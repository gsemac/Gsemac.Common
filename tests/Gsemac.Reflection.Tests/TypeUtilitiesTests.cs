using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Reflection.Tests {

    [TestClass]
    public class TypeUtilitiesTests {

        // Public members

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

        [TestMethod]
        public void TestTryCastWithEnableConstructorAndDefaultConstructor() {

            Assert.IsFalse(TypeUtilities.TryCast("hello world", new CastOptions() {
                EnableConstructor = true,
            }, out ClassWithDefaultConstructor _));

        }
        [TestMethod]
        public void TestTryCastWithEnableConstructorAndMatchingConstructor() {

            Assert.IsTrue(TypeUtilities.TryCast("hello world", new CastOptions() {
                EnableConstructor = true,
            }, out ClassWithStringConstructor result));

            Assert.AreEqual("hello world", result.Text);

        }
        [TestMethod]
        public void TestTryCastWithEnableConstructorAndThrowingConstructor() {

            Assert.IsFalse(TypeUtilities.TryCast("hello world", new CastOptions() {
                EnableConstructor = true,
            }, out ClassWithThrowingConstructor _));

        }

        // Private members

        private class ClassWithDefaultConstructor { }

        private class ClassWithStringConstructor {

            // Public members

            public string Text { get; }

            public ClassWithStringConstructor(string text) {

                Text = text;

            }

        }

        private class ClassWithThrowingConstructor {

            // Public members

            public ClassWithThrowingConstructor(string _) {

                throw new ArgumentException(null, nameof(_));

            }

        }

    }

}