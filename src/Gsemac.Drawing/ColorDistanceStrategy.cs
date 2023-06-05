namespace Gsemac.Drawing {

    public static class ColorDistanceStrategy {

        // Public members

        public static IColorDistanceStrategy DeltaE => new DeltaEColorDistanceStrategy();
        public static IColorDistanceStrategy GrayscaleRgbDifference => new GrayscaleRgbDifferenceColorDistanceStrategy();
        public static IColorDistanceStrategy RgbDifference => new RgbDifferenceColorDistanceStrategy();

    }

}