namespace Gsemac.Data.ValueConversion {

    public class BoolToStringConverter :
        ValueConverterBase<bool, string> {

        // Public members

        public override bool TryConvert(bool value, out string result) {

            if (value)
                result = bool.TrueString.ToLowerInvariant();
            else
                result = bool.FalseString.ToLowerInvariant();

            return true;

        }

    }

}