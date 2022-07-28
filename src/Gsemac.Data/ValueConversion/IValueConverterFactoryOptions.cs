using Gsemac.Reflection;

namespace Gsemac.Data.ValueConversion {

    public interface IValueConverterFactoryOptions {

        ICastOptions CastOptions { get; }

        bool EnableAttributeLookup { get; } 
        bool EnableDerivedClassLookup { get; }
        bool EnableTransitiveLookup { get; }

        bool EnableDefaultConversions { get; }
        bool EnableLookupCache { get; }

    }

}