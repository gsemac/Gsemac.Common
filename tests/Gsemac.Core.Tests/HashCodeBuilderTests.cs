using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Core.Tests {

    [TestClass]
    public class HashCodeBuilderTests {

        // Build

        [TestMethod]
        public void TestSameValuesInDifferentOrdersProducesUniqueHashCodes() {

            int hashCode1 = new HashCodeBuilder()
                .WithValue(1)
                .WithValue(2)
                .WithValue(3)
                .Build();

            int hashCode2 = new HashCodeBuilder()
                .WithValue(3)
                .WithValue(2)
                .WithValue(1)
                .Build();

            Assert.AreNotEqual(hashCode1, hashCode2);

        }

    }

}