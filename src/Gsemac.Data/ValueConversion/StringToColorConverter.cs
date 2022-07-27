using System.Drawing;

namespace Gsemac.Data.ValueConversion {

    public class StringToColorConverter :
           ValueConverterBase<string, Color> {

        // Public members

        public override bool TryConvert(string value, out Color result) {

            result = default;

            if (value is null)
                return false;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            result = ColorConverterUtilities.FromHtml(value);

            return !result.IsEmpty;

        }

    }

}