namespace Gsemac.Drawing {

    public static class ColorDistanceStrategy {

        // Public members

        public static IColorDistanceStrategy DeltaE => new DeltaEStrategy();
        public static IColorDistanceStrategy GreyscaleRgbDifference => new GreyscaleRgbDifferenceStrategy();
        public static IColorDistanceStrategy RgbDifference => new RgbDifferenceStrategy();

    }

}