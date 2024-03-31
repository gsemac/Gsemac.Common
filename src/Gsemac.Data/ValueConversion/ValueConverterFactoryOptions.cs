using Gsemac.Reflection;

namespace Gsemac.Data.ValueConversion {

    public class ValueConverterFactoryOptions :
        IValueConverterFactoryOptions {

        // Public members

        public static ValueConverterFactoryOptions Default => new ValueConverterFactoryOptions();

        public IConvertOptions ConvertOptions {
            get => castOptions;
            set => castOptions = value ?? Reflection.ConvertOptions.Default;
        }

        public bool AttributeLookupEnabled { get; set; } = true;
        public bool DerivedClassLookupEnabled { get; set; } = false;
        public bool TransitiveLookupEnabled { get; set; } = false;

        public bool DefaultConversionsEnabled { get; set; } = true;
        public bool LookupCacheEnabled { get; set; } = true;

        // Private members

        private IConvertOptions castOptions = Reflection.ConvertOptions.Default;

    }

}