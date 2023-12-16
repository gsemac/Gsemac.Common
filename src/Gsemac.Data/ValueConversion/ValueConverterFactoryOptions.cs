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

        public bool AttributeLookupEnabled { get; set; } = true;
        public bool DerivedClassLookupEnabled { get; set; } = false;
        public bool TransitiveLookupEnabled { get; set; } = false;

        public bool DefaultConversionsEnabled { get; set; } = true;
        public bool LookupCacheEnabled { get; set; } = true;

        // Private members

        private ICastOptions castOptions = Reflection.CastOptions.Default;

    }

}