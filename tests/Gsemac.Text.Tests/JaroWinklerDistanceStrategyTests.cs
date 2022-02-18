using Gsemac.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Text.Tests {

    [TestClass]
    public class JaroWinklerDistanceStrategyTests {

        [TestMethod]
        public void TestComputeSimilarityWithSimilarStrings() {

            // The following test cases were found on https://www.geeksforgeeks.org/jaro-and-jaro-winkler-similarity/

            IStringDistanceStrategy strategy = StringDistanceStrategy.JaroWinklerDistance;

            Assert.AreEqual(0.84, strategy.ComputeSimilarity("DwAyNE", "DuANE"), 0.001);
            Assert.AreEqual(0.906667, strategy.ComputeSimilarity("TRATE", "TRACE"), 0.001);

        }
        [TestMethod]
        public void TestComputeSimilarityWithDissimilarStrings() {

            IStringDistanceStrategy strategy = StringDistanceStrategy.JaroWinklerDistance;

            Assert.AreEqual(0.0, strategy.ComputeSimilarity("hello world", string.Empty));
            Assert.AreEqual(0.0, strategy.ComputeSimilarity(string.Empty, "hello world"));
            Assert.AreEqual(0.0, strategy.ComputeSimilarity("zebra", "list"));

        }
        [TestMethod]
        public void TestComputeSimilarityWithIdenticalStrings() {

            IStringDistanceStrategy strategy = StringDistanceStrategy.JaroWinklerDistance;

            Assert.AreEqual(1.0, strategy.ComputeSimilarity(string.Empty, string.Empty));
            Assert.AreEqual(1.0, strategy.ComputeSimilarity("hello world", "hello world"));

        }

    }

}