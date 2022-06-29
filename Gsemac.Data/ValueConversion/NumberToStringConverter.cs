using System;
using System.Globalization;

namespace Gsemac.Data.ValueConversion {

    public class NumberToStringConverter<TNumber> :
        ValueConverterBase<TNumber, string> {

        // Public members

        public NumberToStringConverter() :
            this(CultureInfo.InvariantCulture) {
        }
        public NumberToStringConverter(IFormatProvider formatProvider) {

            if (formatProvider is null)
                throw new ArgumentNullException(nameof(formatProvider));

            this.formatProvider = formatProvider;

        }

        public override bool TryConvert(TNumber value, out string result) {

            result = default;

            switch (value) {

                case byte byteValue:
                    result = byteValue.ToString(formatProvider);
                    break;

                case sbyte sbyteValue:
                    result = sbyteValue.ToString(formatProvider);
                    break;

                case decimal decimalValue:
                    result = decimalValue.ToString(formatProvider);
                    break;

                case double doubleValue:
                    result = doubleValue.ToString(formatProvider);
                    break;

                case float floatValue:
                    result = floatValue.ToString(formatProvider);
                    break;

                case int intValue:
                    result = intValue.ToString(formatProvider);
                    break;

                case uint uintValue:
                    result = uintValue.ToString(formatProvider);
                    break;

                case long longValue:
                    result = longValue.ToString(formatProvider);
                    break;

                case ulong ulongValue:
                    result = ulongValue.ToString(formatProvider);
                    break;

                case short shortValue:
                    result = shortValue.ToString(formatProvider);
                    break;

                case ushort ushortValue:
                    result = ushortValue.ToString(formatProvider);
                    break;

                default:
                    return false;

            }

            return true;

        }

        // Private members

        private readonly IFormatProvider formatProvider;

    }

}