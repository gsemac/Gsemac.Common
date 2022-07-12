namespace Gsemac.Data.ValueConversion {

    public interface IValueConverterFactoryOptions {

        bool EnableTransitiveConversion { get; }
        bool EnableDefaultConverters { get; }

    }

}