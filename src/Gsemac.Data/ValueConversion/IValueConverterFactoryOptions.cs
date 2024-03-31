using Gsemac.Reflection;

namespace Gsemac.Data.ValueConversion {

    public interface IValueConverterFactoryOptions {

        IConvertOptions ConvertOptions { get; }

        bool AttributeLookupEnabled { get; }
        bool DerivedClassLookupEnabled { get; }
        bool TransitiveLookupEnabled { get; }

        bool DefaultConversionsEnabled { get; }
        bool LookupCacheEnabled { get; }

    }

}