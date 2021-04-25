using Gsemac.Reflection;
using Gsemac.Reflection.Extensions;
using System;
using System.Globalization;

namespace Gsemac.Text.Ini.Extensions {

    public static class IniSectionExtensions {

        public static void AddProperty(this IIniSection iniSection, string name, string value) {

            iniSection.AddProperty(new IniProperty(name, value));

        }
        public static bool HasProperty(this IIniSection iniSection, string name) {

            return !(iniSection.GetProperty(name) is null);

        }

        public static bool TryGetValue<T>(this IIniSection iniSection, string propertyName, out T propertyValue) {

            IIniProperty property = iniSection.GetProperty(propertyName);

            // Only attempt to convert the string if the property isn't empty, because some types will get an unexpected value (e.g. bools will be false).

            if (!string.IsNullOrWhiteSpace(property?.Value)) {

                try {

                    return TypeUtilities.TryCast(property.Value, out propertyValue);

                }
                catch (FormatException) { }

            }

            propertyValue = default;

            return false;

        }
        public static string GetValue(this IIniSection iniSection, string propertyName) {

            return iniSection.GetValue<string>(propertyName);

        }
        public static T GetValue<T>(this IIniSection iniSection, string propertyName) {

            iniSection.TryGetValue(propertyName, out T propertyValue);

            return propertyValue;

        }
        public static void SetValue<T>(this IIniSection iniSection, string propertyName, T propertyValue) {

            iniSection.AddProperty(new IniProperty(propertyName, Convert.ToString(propertyValue, CultureInfo.InvariantCulture)));

        }

    }

}