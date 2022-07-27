using System.Drawing;

namespace Gsemac.Data.ValueConversion {

    public class ColorToStringConverter :
        ValueConverterBase<Color, string> {

        // Public members

        public override bool TryConvert(Color value, out string result) {

            result = ColorConverterUtilities.ToHtml(value);

            // Treat "empty" colors the same as black.
            // Even though they are black as far as their RGB values are concerned, ColorTranslator will serialize them as the empty string.

            if (string.IsNullOrWhiteSpace(result))
                result = ColorConverterUtilities.ToHtml(Color.Black);

            result = result.ToLowerInvariant();

            return true;

        }

    }

}