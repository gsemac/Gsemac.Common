namespace Gsemac.Data.ValueConversion {

    public class ValueConverterFactory :
        ValueConverterFactoryBase {

        // Public members

        public static ValueConverterFactory Default { get; } = new ValueConverterFactory();

        public ValueConverterFactory() { }
        public ValueConverterFactory(IValueConverterFactoryOptions options) :
            base(options) {
        }

    }

}