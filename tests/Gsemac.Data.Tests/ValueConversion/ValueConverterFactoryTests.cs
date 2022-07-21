using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Data.ValueConversion.Tests {

    [TestClass]
    public class ValueConverterFactoryTests {

        // Create

        [TestMethod]
        public void TestInvalidConversionWithTransitiveConversionsEnabled() {

            // Invalid conversions should fail normally (return false) even if we fail to find a valid transitive conversion path.
            // In other words, failing to find a transitive conversion path should not throw an exception.

            ValueConverterFactory factory = new(new ValueConverterFactoryOptions() {
                EnableTransitiveConversion = true,
            });

            Assert.IsFalse(factory.Create<string, ValueConverterFactoryTests>().TryConvert("abc", out object _));

        }

    }

}