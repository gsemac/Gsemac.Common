using Gsemac.Reflection;

namespace Gsemac.Data.ValueConversion {

    public interface IValueConverterFactoryOptions {

        ICastOptions CastOptions { get; }
        bool EnableTransitiveConversion { get; }
        bool EnableDefaultConverters { get; }

    }

}