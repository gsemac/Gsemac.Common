namespace Gsemac.Core {

    public static class RangeFormatter {

        // Public members

        public static IRangeFormatter Bounded => new BoundedRangeFormatter();
        public static IRangeFormatter Dashed => new DashedRangeFormatter();

    }

}