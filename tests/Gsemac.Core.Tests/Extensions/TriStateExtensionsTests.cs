using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Core.Extensions.Tests {

    [TestClass]
    public class TriStateExtensionsTests {

        // ToBoolean

        [TestMethod]
        public void TestToBooleanWithTruthyValue() {

            Assert.IsTrue(TriState.True.ToBoolean());

        }
        [TestMethod]
        public void TestToBooleanWithFalsyValue() {

            Assert.IsFalse(TriState.False.ToBoolean());

        }
        [TestMethod]
        public void TestToBooleanWithNullValue() {

            Assert.IsNull(TriState.Unset.ToBoolean());

        }

    }

}