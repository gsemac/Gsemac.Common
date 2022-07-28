using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class ValueConverterFactoryTests {

        // Public members

        // Create

        [TestMethod]
        public void TestConversionToInterfaceWithMultipleCandidatesUsesLastCandidate() {

            TestValueConverterFactory factory = new TestValueConverterFactory();

            factory.AddValueConverter(ValueConverter.Create<string, TestClassWithConstructor>(str => new TestClassWithConstructor("bad")));
            factory.AddValueConverter(ValueConverter.Create<string, TestClassWithConstructor>(str => new TestClassWithConstructor("good")));

            Assert.IsTrue(factory.Create<string, TestClassWithConstructor>().TryConvert("abc", out TestClassWithConstructor result));
            Assert.AreEqual("good", result.Value);

        }

        [TestMethod]
        public void TestTransitiveConversionWithTransitiveLookupDisabled() {

            TestValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableTransitiveLookup = false,
            });

            factory.AddValueConverter(ValueConverter.Create<int, string>(arg => arg.ToString(CultureInfo.InvariantCulture)));
            factory.AddValueConverter(ValueConverter.Create<string, TestClassWithConstructor>(arg => new TestClassWithConstructor(arg)));

            Assert.IsFalse(factory.Create<int, TestClassWithConstructor>().TryConvert(5, out TestClassWithConstructor _));

        }
        [TestMethod]
        public void TestTransitiveConversionWithTransitiveLookupEnabled() {

            TestValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableTransitiveLookup = true,
            });

            factory.AddValueConverter(ValueConverter.Create<int, string>(arg => arg.ToString(CultureInfo.InvariantCulture)));
            factory.AddValueConverter(ValueConverter.Create<string, TestClassWithConstructor>(arg => new TestClassWithConstructor(arg)));

            Assert.IsTrue(factory.Create<int, TestClassWithConstructor>().TryConvert(5, out TestClassWithConstructor result));
            Assert.AreEqual("5", result.Value);

        }
        [TestMethod]
        public void TestInvalidConversionWithTransitiveLookupEnabledDoesNotThrowException() {

            // Invalid conversions should fail normally (return false) even if we fail to find a valid transitive conversion path.
            // In other words, failing to find a transitive conversion path should not throw an exception.

            ValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableTransitiveLookup = true,
            });

            Assert.IsFalse(factory.Create<string, ValueConverterFactoryTests>().TryConvert("abc", out object _));

        }
        [TestMethod]
        public void TestTransitiveConversionToInterfaceWithTransitiveLookupEnabledAndDerivedClassLookupEnabled() {

            // When both transitive and derived class lookup are enabled, we should be able to find a transitive path to the derived class type.

            TestValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableTransitiveLookup = true,
                EnableDerivedClassLookup = true,
            });

            factory.AddValueConverter(ValueConverter.Create<int, string>(arg => arg.ToString(CultureInfo.InvariantCulture)));
            factory.AddValueConverter(ValueConverter.Create<string, TestClassWithConstructor>(str => new TestClassWithConstructor(str)));

            Assert.IsTrue(factory.Create<int, ITestInterface>().TryConvert(5, out ITestInterface result));
            Assert.AreEqual(typeof(TestClassWithConstructor), result.GetType());
            Assert.AreEqual("5", ((TestClassWithConstructor)result).Value);

        }
        [TestMethod]
        public void TestTransitiveConversionToInterfaceWitWithTransitiveLookupEnabledAndDerivedClassLookupEnabledAndMultipleCandidatesUsesLastCandidate() {

            TestValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableTransitiveLookup = true,
                EnableDerivedClassLookup = true,
            });

            factory.AddValueConverter(ValueConverter.Create<int, string>(arg => arg.ToString(CultureInfo.InvariantCulture)));
            factory.AddValueConverter(ValueConverter.Create<string, TestClassWithConstructor>(str => new TestClassWithConstructor("bad")));
            factory.AddValueConverter(ValueConverter.Create<string, TestClassWithConstructor>(str => new TestClassWithConstructor("good")));

            Assert.IsTrue(factory.Create<int, ITestInterface>().TryConvert(5, out ITestInterface result));
            Assert.AreEqual(typeof(TestClassWithConstructor), result.GetType());
            Assert.AreEqual("good", ((TestClassWithConstructor)result).Value);

        }

        [TestMethod]
        public void TestConversionToInterfaceWithDerivedClassLookupDisabled() {

            TestValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableDerivedClassLookup = false,
            });

            factory.AddValueConverter(ValueConverter.Create<string, TestClassImplementingInterface>(str => new TestClassImplementingInterface()));

            Assert.IsFalse(factory.Create<string, ITestInterface>().TryConvert("abc", out ITestInterface _));

        }
        [TestMethod]
        public void TestConversionToInterfaceWithDerivedClassLookupEnabled() {

            TestValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableDerivedClassLookup = true,
            });

            factory.AddValueConverter(ValueConverter.Create<string, TestClassImplementingInterface>(str => new TestClassImplementingInterface()));

            Assert.IsTrue(factory.Create<string, ITestInterface>().TryConvert("abc", out ITestInterface _));

        }
        [TestMethod]
        public void TestConversionToInterfaceWithDerivedClassLookupEnabledAndMultipleCandidatesIgnoresFailedConversions() {

            TestValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableDerivedClassLookup = true,
            });

            factory.AddValueConverter(ValueConverter.Create<string, TestClassImplementingInterface>(str => throw new Exception()));
            factory.AddValueConverter(ValueConverter.Create<string, TestClassImplementingInterface>(str => new TestClassImplementingInterface()));

            Assert.IsTrue(factory.Create<string, ITestInterface>().TryConvert("abc", out ITestInterface _));

        }
        [TestMethod]
        public void TestConversionToInterfaceWithDerivedClassLookupEnabledAndMultipleCandidatesUsesLastCandidate() {

            TestValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableDerivedClassLookup = true,
            });

            factory.AddValueConverter(ValueConverter.Create<string, TestClassWithConstructor>(str => new TestClassWithConstructor("bad")));
            factory.AddValueConverter(ValueConverter.Create<string, TestClassWithConstructor>(str => new TestClassWithConstructor("good")));

            Assert.IsTrue(factory.Create<string, ITestInterface>().TryConvert("abc", out ITestInterface result));
            Assert.AreEqual(typeof(TestClassWithConstructor), result.GetType());
            Assert.AreEqual("good", ((TestClassWithConstructor)result).Value);

        }

        [TestMethod]
        public void TestConversionWithValueConverterAttribute() {

            TestValueConverterFactory factory = new TestValueConverterFactory(new ValueConverterFactoryOptions() {
                EnableAttributeLookup = true,
            });

            Assert.IsTrue(factory.Create<string, TestClassWithAttribute>().TryConvert("good", out TestClassWithAttribute result));
            Assert.AreEqual("good", result.Value);

        }

        // Private members

        private interface ITestInterface { }

        private class TestClassImplementingInterface :
            ITestInterface {
        }

        private class TestClassWithConstructor :
            ITestInterface {

            // Public members

            public string Value { get; }

            public TestClassWithConstructor(string value) {

                Value = value;

            }

        }

        [ValueConverter(typeof(TestClassWithAttributeConverter))]
        private class TestClassWithAttribute :
            ITestInterface {

            // Public members

            public string Value { get; }

            public TestClassWithAttribute(string value) {

                Value = value;

            }

        }

        private class TestClassWithAttributeConverter :
            ValueConverterBase<string, TestClassWithAttribute> {

            // Public members

            public override bool TryConvert(string value, out TestClassWithAttribute result) {

                result = new TestClassWithAttribute(value);

                return true;

            }

        }

        private class TestValueConverterFactory :
            ValueConverterFactory {

            // Public members

            public TestValueConverterFactory() {
            }
            public TestValueConverterFactory(IValueConverterFactoryOptions options) :
                base(options) {
            }

            public new void AddValueConverter(IValueConverter valueConverter) {

                if (valueConverter is null)
                    throw new ArgumentNullException(nameof(valueConverter));

                base.AddValueConverter(valueConverter);

            }

        }

    }

}