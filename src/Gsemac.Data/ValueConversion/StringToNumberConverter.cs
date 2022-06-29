using System;
using System.Globalization;

namespace Gsemac.Data.ValueConversion {

    public class StringToNumberConverter<TNumber> :
        ValueConverterBase<string, TNumber> {

        // Public members

        public StringToNumberConverter() :
            this(CultureInfo.InvariantCulture) {
        }
        public StringToNumberConverter(IFormatProvider formatProvider) {

            if (formatProvider is null)
                throw new ArgumentNullException(nameof(formatProvider));

            this.formatProvider = formatProvider;

        }

        public override bool TryConvert(string value, out TNumber result) {

            result = default;

            object tempResult = default;

            switch (result) {

                case byte _:

                    if (byte.TryParse(value, NumberStyles.Integer, formatProvider, out byte byteValue))
                        tempResult = byteValue;

                    break;

                case sbyte _:

                    if (sbyte.TryParse(value, NumberStyles.Integer, formatProvider, out sbyte sbyteValue))
                        tempResult = sbyteValue;

                    break;

                case decimal _:

                    if (decimal.TryParse(value, NumberStyles.Float, formatProvider, out decimal decimalValue))
                        tempResult = decimalValue;

                    break;

                case double _:

                    if (double.TryParse(value, NumberStyles.Float, formatProvider, out double doubleValue))
                        tempResult = doubleValue;

                    break;

                case float _:

                    if (float.TryParse(value, NumberStyles.Float, formatProvider, out float floatValue))
                        tempResult = floatValue;

                    break;

                case int _:

                    if (int.TryParse(value, NumberStyles.Integer, formatProvider, out int intValue))
                        tempResult = intValue;

                    break;

                case uint _:

                    if (uint.TryParse(value, NumberStyles.Integer, formatProvider, out uint uintValue))
                        tempResult = uintValue;

                    break;

                case long _:

                    if (long.TryParse(value, NumberStyles.Integer, formatProvider, out long longValue))
                        tempResult = longValue;

                    break;

                case ulong _:

                    if (ulong.TryParse(value, NumberStyles.Integer, formatProvider, out ulong ulongValue))
                        tempResult = ulongValue;

                    break;

                case short _:

                    if (short.TryParse(value, NumberStyles.Integer, formatProvider, out short shortValue))
                        tempResult = shortValue;

                    break;

                case ushort _:

                    if (ushort.TryParse(value, NumberStyles.Integer, formatProvider, out ushort ushortValue))
                        tempResult = ushortValue;

                    break;

                default:
                    return false;

            }

            // If we successfully converted the number, assign it to the result.

            bool success = !(tempResult is null);

            if (success)
                result = (TNumber)System.Convert.ChangeType(tempResult, typeof(TNumber));

            return success;

        }

        // Private members

        private readonly IFormatProvider formatProvider;

    }

}