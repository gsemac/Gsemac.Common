using Gsemac.Reflection;

namespace Gsemac.Data.ValueConversion {

    public interface IValueConverterFactoryOptions {

        ICastOptions CastOptions { get; }

        bool EnableDerivedClassLookup { get; }
        bool EnableTransitiveLookup { get; }

        bool EnableDefaultConversions { get; }

        bool EnableLookupCache { get; }

    }

}