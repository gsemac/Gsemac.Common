﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Gsemac.Drawing.Tests {

    [TestClass]
    public abstract class ColorDistanceStrategyTestsBase {

        // Public members

        [TestMethod]
        public void TestComputeDistanceReturnsMaximumDistanceForBlackAndWhite() {

            Assert.AreEqual(1.0, colorDistanceStrategy.ComputeDistance(Color.Black, Color.White, normalize: true));

        }
        [TestMethod]
        public void TestComputeDistanceReturnsMinimumDistanceForSameColors() {

            Assert.AreEqual(0.0, colorDistanceStrategy.ComputeDistance(Color.Red, Color.Red, normalize: true));

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
            base(new RgbDifferenceColorDistanceStrategy()) {
        }

    }

    [TestClass]
    public class GrayscaleRgbDifferenceColorDistanceStrategyTests :
      ColorDistanceStrategyTestsBase {

        public GrayscaleRgbDifferenceColorDistanceStrategyTests() :
            base(new GrayscaleRgbDifferenceColorDistanceStrategy()) {
        }

    }

    [TestClass]
    public class DeltaEColorDistanceStrategyTests :
      ColorDistanceStrategyTestsBase {

        public DeltaEColorDistanceStrategyTests() :
            base(new DeltaEColorDistanceStrategy()) {
        }

    }

}