using Gsemac.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Text.Tests {

    [TestClass]
    public class JaroDistanceStrategyTests {

        [TestMethod]
        public void TestComputeSimilarityWithSimilarStrings() {

            // The following test cases were found on https://rosettacode.org/wiki/Jaro_similarity#C.2B.2B

            IStringDistanceStrategy strategy = StringDistanceStrategy.JaroDistance;

            Assert.AreEqual(0.9444444444, strategy.ComputeSimilarity("MARTHA", "MARHTA"), 0.001);
            Assert.AreEqual(0.7666666667, strategy.ComputeSimilarity("DIXON", "DICKSONX"), 0.001);
            Assert.AreEqual(0.8962962963, strategy.ComputeSimilarity("JELLYFISH", "SMELLYFISH"), 0.001);

        }
        [TestMethod]
        public void TestComputeSimilarityWithDissimilarStrings() {

            IStringDistanceStrategy strategy = StringDistanceStrategy.JaroDistance;

            Assert.AreEqual(0.0, strategy.ComputeSimilarity("hello world", string.Empty));
            Assert.AreEqual(0.0, strategy.ComputeSimilarity(string.Empty, "hello world"));
            Assert.AreEqual(0.0, strategy.ComputeSimilarity("zebra", "list"));

        }
        [TestMethod]
        public void TestComputeSimilarityWithIdenticalStrings() {

            IStringDistanceStrategy strategy = StringDistanceStrategy.JaroDistance;

            Assert.AreEqual(1.0, strategy.ComputeSimilarity(string.Empty, string.Empty));
            Assert.AreEqual(1.0, strategy.ComputeSimilarity("hello world", "hello world"));

        }

    }

}