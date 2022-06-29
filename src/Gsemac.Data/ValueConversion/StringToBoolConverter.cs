namespace Gsemac.Data.ValueConversion {

    public class StringToBoolConverter :
        ValueConverterBase<string, bool> {

        // Public members

        public override bool TryConvert(string value, out bool result) {

            return bool.TryParse(value, out result);

        }

    }

}