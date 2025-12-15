using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Reflection.Plugins.Tests {
    [TestClass]
    public class RequiresAssembliesAttributeTests {
        [TestMethod]
        public void TestTestRequirementWithNullServiceProviderDoesNotThrow() {
            // Passing in a null service provider shouldn't throw any exceptions.
            Assert.IsFalse(new RequiresAssembliesAttribute("hello", "world").TestRequirement(null));
        }
    }
}