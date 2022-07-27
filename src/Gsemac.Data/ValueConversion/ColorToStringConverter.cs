using System.Drawing;

namespace Gsemac.Data.ValueConversion {

    public class ColorToStringConverter :
        ValueConverterBase<Color, string> {

        // Public members

        public override bool TryConvert(Color value, out string result) {

            result = ColorConverterUtilities.ToHtml(value)
                .ToLowerInvariant();

            return true;

        }

    }

}