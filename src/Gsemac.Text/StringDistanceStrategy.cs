namespace Gsemac.Text {

    public static class StringDistanceStrategy {

        // Public members

        public static IStringDistanceStrategy HammingDistance => new HammingDistanceStrategy();
        public static IStringDistanceStrategy JaroDistance => new JaroDistanceStrategy();
        public static IStringDistanceStrategy JaroWinklerDistance => new JaroWinklerDistanceStrategy();
        public static IStringDistanceStrategy LevenshteinDistance => new LevenshteinDistanceStrategy();

    }

}