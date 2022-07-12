namespace Gsemac.Data.ValueConversion {

    public class ValueConverterFactoryOptions :
        IValueConverterFactoryOptions {

        // Public members

        public static ValueConverterFactoryOptions Default => new ValueConverterFactoryOptions();

        public bool EnableTransitiveConversion { get; set; } = false;
        public bool EnableDefaultConverters { get; set; } = true;

    }

}