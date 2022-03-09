namespace Gsemac.Drawing {

    public static class ColorDistanceStrategy {

        // Public members

        public static IColorDistanceStrategy DeltaE => new DeltaEStrategy();
        public static IColorDistanceStrategy GrayscaleRgbDifference => new GrayscaleRgbDifferenceStrategy();
        public static IColorDistanceStrategy RgbDifference => new RgbDifferenceStrategy();

    }

}