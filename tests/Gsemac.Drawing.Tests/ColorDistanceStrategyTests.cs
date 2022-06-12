using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Gsemac.Drawing.Tests {

    [TestClass]
    public abstract class ColorDistanceStrategyTestsBase {

        // Public members

        [TestMethod]
        public void TestComputeDistanceReturnsMaximumDistanceForBlackAndWhite() {

            Assert.AreEqual(1.0, colorDistanceStrategy.ComputeDistance(Color.Black, Color.White, normalizeResult: true));

        }
        [TestMethod]
        public void TestComputeDistanceReturnsMinimumDistanceForSameColors() {

            Assert.AreEqual(0.0, colorDistanceStrategy.ComputeDistance(Color.Red, Color.Red, normalizeResult: true));

        }

        // Protected members

        protected ColorDistanceStrategyTestsBase(IColorDistanceStrategy colorDistanceStrategy) {

            this.colorDistanceStrategy = colorDistanceStrategy;

        }

        // Private members

        private readonly IColorDistanceStrategy colorDistanceStrategy;

    }

    [TestClass]
    public class RgbDifferenceColorDistanceStrategyTests :
      ColorDistanceStrategyTestsBase {

        public RgbDifferenceColorDistanceStrategyTests() :
            base(new RgbDifferenceStrategy()) {
        }

    }

    [TestClass]
    public class GrayscaleRgbDifferenceColorDistanceStrategyTests :
      ColorDistanceStrategyTestsBase {

        public GrayscaleRgbDifferenceColorDistanceStrategyTests() :
            base(new GrayscaleRgbDifferenceStrategy()) {
        }

    }

    [TestClass]
    public class DeltaEColorDistanceStrategyTests :
      ColorDistanceStrategyTestsBase {

        public DeltaEColorDistanceStrategyTests() :
            base(new DeltaEStrategy()) {
        }

    }

}