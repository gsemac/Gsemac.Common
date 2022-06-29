using Gsemac.Data.ValueConversion;

namespace Gsemac.Text.Ini.ValueConversion {

    internal class IniValueConverterFactory :
        ValueConverterFactoryBase {

        // Public members

        public IniValueConverterFactory() {

            AddValueConverter(new IniStringToBoolConverter());

        }

    }

}