using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class ValueConverterFactoryTests {

        // Public members

        // Create

        [TestMethod]
        public void TestInvalidConversionWithTransitiveLookupEnabledDoesNotThrowException() {

            // Invalid conversions should fail normally (return false) even if we fail to find a valid transitive conversion path.
            // In other words, failing to find a transitive conversion path should not throw an exception.

            ValueConverterFactory factory = new(new ValueConverterFactoryOptions() {
                EnableTransitiveLookup = true,
            });

            Assert.IsFalse(factory.Create<string, ValueConverterFactoryTests>().TryConvert("abc", out object _));

        }

        [TestMethod]
        public void TestConvertToInterfaceWithDerivedClassLookupDisabled() {

            TestValueConverterFactory factory = new(new ValueConverterFactoryOptions() {
                EnableDerivedClassLookup = false,
            });

            factory.AddValueConverter(ValueConverter.Create<string, TestClassImplementingInterface>(str => new TestClassImplementingInterface()));

            Assert.IsFalse(factory.Create<string, ITestInterface>().TryConvert("abc", out ITestInterface _));

        }
        [TestMethod]
        public void TestConvertToInterfaceWithDerivedClassLookupEnabled() {

            TestValueConverterFactory factory = new(new ValueConverterFactoryOptions() {
                EnableDerivedClassLookup = true,
            });

            factory.AddValueConverter(ValueConverter.Create<string, TestClassImplementingInterface>(str => new TestClassImplementingInterface()));

            Assert.IsTrue(factory.Create<string, ITestInterface>().TryConvert("abc", out ITestInterface _));

        }

        // Private members

        private interface ITestInterface { }

        private class TestClassImplementingInterface :
            ITestInterface {
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