﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Reflection.Tests {

    [TestClass]
    public class TypeUtilitiesTests {

        // Public members

        // TryCast

        [TestMethod]
        public void TestTryCastWithValidCast() {

            Assert.IsTrue(TypeUtilities.TryCast("35", out int result));
            Assert.AreEqual(35, result);

        }
        [TestMethod]
        public void TestTryCastWithInvalidCast() {

            Assert.IsFalse(TypeUtilities.TryCast("abc", out int _));

        }

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
        public void TestTryCastWithEnableConstructorAndDefaultConstructor() {

            Assert.IsFalse(TypeUtilities.TryCast("hello world", new CastOptions() {
                EnableConstructorInitialization = true,
            }, out TestClassWithDefaultConstructor _));

        }
        [TestMethod]
        public void TestTryCastWithEnableConstructorAndMatchingConstructor() {

            Assert.IsTrue(TypeUtilities.TryCast("hello world", new CastOptions() {
                EnableConstructorInitialization = true,
            }, out TestClassWithStringConstructor result));

            Assert.AreEqual("hello world", result.Text);

        }
        [TestMethod]
        public void TestTryCastWithEnableConstructorAndThrowingConstructor() {

            Assert.IsFalse(TypeUtilities.TryCast("hello world", new CastOptions() {
                EnableConstructorInitialization = true,
            }, out TestClassWithThrowingConstructor _));

        }

        [TestMethod]
        public void TestTryCastToInterfaceWithClassImplementingInterface() {

            object obj = new TestClassImplementingInterface();

            Assert.IsTrue(TypeUtilities.TryCast(obj, out ITestInterface result));
            Assert.IsTrue(ReferenceEquals(obj, result));

        }
        [TestMethod]
        public void TestTryCastToSameType() {

            object obj = new TestClassWithDefaultConstructor();

            Assert.IsTrue(TypeUtilities.TryCast(obj, out TestClassWithDefaultConstructor result));
            Assert.IsTrue(ReferenceEquals(obj, result));

        }
        [TestMethod]
        public void TestTryCastCustomClassToString() {

            object obj = new TestClassWithDefaultConstructor();

            Assert.IsTrue(TypeUtilities.TryCast(obj, out string result));
            Assert.AreEqual(obj.ToString(), result);

        }

        // Private members

        private interface ITestInterface { }

        private class TestClassImplementingInterface :
            ITestInterface {
        }

        private class TestClassWithDefaultConstructor { }

        private class TestClassWithStringConstructor {

            // Public members

            public string Text { get; }

            public TestClassWithStringConstructor(string text) {

                Text = text;

            }

        }

        private class TestClassWithThrowingConstructor {

            // Public members

            public TestClassWithThrowingConstructor(string _) {

                throw new ArgumentException(null, nameof(_));

            }

        }

    }

}