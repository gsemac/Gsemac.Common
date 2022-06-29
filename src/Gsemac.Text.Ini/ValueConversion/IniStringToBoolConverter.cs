using Gsemac.Data.ValueConversion;

namespace Gsemac.Text.Ini.ValueConversion {

    internal class IniStringToBoolConverter :
        ValueConverterBase<string, bool> {

        // Public members

        public override bool TryConvert(string value, out bool result) {

            result = default;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            switch (value.Trim().ToLowerInvariant()) {

                case "true":
                case "on":
                case "yes":
                case "1":
                    result = true;
                    break;

                case "false":
                case "off":
                case "no":
                case "0":
                    result = false;
                    break;

                default:
                    return false;

            }

            return true;
        }

    }

}