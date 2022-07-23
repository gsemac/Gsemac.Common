using Gsemac.Reflection;

namespace Gsemac.Data.ValueConversion {

    public class ValueConverterFactoryOptions :
        IValueConverterFactoryOptions {

        // Public members

        public static ValueConverterFactoryOptions Default => new ValueConverterFactoryOptions();

        public ICastOptions CastOptions {
            get => castOptions;
            set => castOptions = value ?? Reflection.CastOptions.Default;
        }

        public bool EnableDerivedClassLookup { get; set; } = false;
        public bool EnableTransitiveLookup { get; set; } = false;

        public bool EnableDefaultConverters { get; set; } = true;

        // Private members

        private ICastOptions castOptions = Reflection.CastOptions.Default;

    }

}