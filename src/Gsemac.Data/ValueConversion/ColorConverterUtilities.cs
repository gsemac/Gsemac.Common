using Gsemac.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gsemac.Data.ValueConversion {

    internal class ColorConverterUtilities {

        // Public members

        public static string ToHtml(Color value) {

            // If the color is transparent, just return the "transparent" keyword.
            // "Empty" colors will also be treated as transparent, because they have the RGBA value (0, 0, 0, 0).
            // This makes them transparent when used in graphics routines.

            if (value.IsEmpty || value.A <= 0)
                return Color.Transparent.Name;

            // If this color has an explicit name, return the name of the color.

            if (TryGetColorName(value, out string result))
                return result;

            // If this color is translucent, return an 8-digit hex string with alpha.
            // https://css-tricks.com/8-digit-hex-codes/

            if (value.A < byte.MaxValue)
                return $"#{value.R:X2}{value.G:X2}{value.B:X2}{value.A:X2}";

            // For all other colors, return a 6-digit hex string.

#if NETFRAMEWORK

            // System.Drawing does not contain the ColorTranslator class in .NET Core and above. 
            // It is capable of returning system colors for the (obsolete) CSS system color keywords.

            return ColorTranslator.ToHtml(value);

#else

            return $"#{value.R:X2}{value.G:X2}{value.B:X2}";

#endif

        }
        public static Color FromHtml(string value) {

            if (string.IsNullOrWhiteSpace(value))
                return Color.Empty;

            value = value.ToLowerInvariant().Trim();

            // If we were given an explicit color name, try looking up the color by name.

            if (TryGetColor(value, out Color result))
                return result;

            result = Color.Empty;

            if (value.StartsWith("#")) {

                // Normalize the hex string to an even number of digits of length 6 or 8 (with alpha).

                value = NormalizeHexString(value)
                    .Substring(1);

                if (value.Length != 6 && value.Length != 8)
                    return Color.Empty;

                // Parse the hex string into individual RGBA values.

                int r = 0;
                int g = 0;
                int b = 0;
                int a = 255;

                bool parsedSuccessfully = int.TryParse(value.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out r) &&
                    int.TryParse(value.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out g) &&
                    int.TryParse(value.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b);

                if (value.Length == 8)
                    parsedSuccessfully &= int.TryParse(value.Substring(6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out a);

                if (parsedSuccessfully)
                    result = Color.FromArgb(a, r, g, b);

            }
#if NETFRAMEWORK
            else {

                try {

                    // System.Drawing does not contain the ColorTranslator class in .NET Core and above. 
                    // It is capable of returning system colors for the (obsolete) CSS system color keywords.

                    result = ColorTranslator.FromHtml(value);

                }
                catch (Exception) { }

            }
#endif

            // If parsing was successful but the color doesn't have a name, try to map it to one of the named colors.
            // This allows for equality comparisons like "Color.Red.Equals(result)".

            if (!result.Equals(Color.Empty) && TryGetColorName(result, out string colorName))
                return FromHtml(colorName);

            return result;

        }

        // Private members

        private class ArgbColorEqualityComparer :
            IEqualityComparer<Color> {

            // Public members

            public bool Equals(Color x, Color y) {

                return GetHashCode(x).Equals(GetHashCode(y));

            }
            public int GetHashCode(Color obj) {

                return new HashCodeBuilder()
                    .WithValue(obj.R)
                    .WithValue(obj.G)
                    .WithValue(obj.B)
                    .WithValue(obj.A)
                    .Build();

            }

        }

        private static readonly Lazy<IDictionary<string, Color>> namedColors = new Lazy<IDictionary<string, Color>>(BuildNamedColorsDict);
        private static readonly Lazy<IDictionary<Color, string>> namedColorNames = new Lazy<IDictionary<Color, string>>(BuildNamedColorNamesDict);

        private static IDictionary<string, Color> BuildNamedColorsDict() {

            var pairs = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(property => property.PropertyType == typeof(Color))
                .Select(property => (Color)property.GetValue(null, null))
                .Select(color => new KeyValuePair<string, Color>(color.Name, color));

            IDictionary<string, Color> result = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);

            // We shouldn't encounter duplicate color names, but just in case, avoid using the Add method.

            foreach (var pair in pairs)
                result[pair.Key] = pair.Value;

            // The color "rebeccapurple" (#663399) was added in CSS4.
            // https://en.wikipedia.org/wiki/Eric_A._Meyer#Personal_life
            // If we're compiling against .NET 6.0 or later, it will already be present in the dictionary.

            result["rebeccapurple"] = Color.FromArgb(102, 51, 153);

            return result;

        }
        private static IDictionary<Color, string> BuildNamedColorNamesDict() {

            // We may run into duplicate keys because of color aliases (e.g. "magenta" and "fuchsia").
            // https://developer.mozilla.org/en-US/docs/Web/CSS/color_value/color_keywords

            // Prefer whichever one was defined first in the CSS standard.

            IDictionary<Color, string> result = new Dictionary<Color, string>(new ArgbColorEqualityComparer());

            foreach (var pair in namedColors.Value) {

                if (result.ContainsKey(pair.Value))
                    continue;

                result.Add(pair.Value, pair.Key);

            }

            return result;

        }

        private static bool TryGetColor(string value, out Color result) {

            // Normalize the spelling of "grey".
            // CSS colors accept both variants as aliases of one another.

            value = value.ToLowerInvariant()
                .Replace("grey", "gray");

            return namedColors.Value.TryGetValue(value, out result);

        }
        private static bool TryGetColorName(Color value, out string result) {

            if (value.IsNamedColor) {

                result = value.Name;

                return true;

            }

            return namedColorNames.Value.TryGetValue(value, out result);

        }

        private static string NormalizeHexString(string value) {

            if (string.IsNullOrWhiteSpace(value))
                return value;

            int length = value.Length;

            if (value.StartsWith("#"))
                length -= 1;

            if (length < 3 || length % 2 == 0)
                return value;

            StringBuilder sb = new StringBuilder();

            if (value.StartsWith("#"))
                sb.Append('#');

            foreach (char c in value.Skip(1)) {

                sb.Append(c);
                sb.Append(c);

            }

            return sb.ToString();

        }

    }

}