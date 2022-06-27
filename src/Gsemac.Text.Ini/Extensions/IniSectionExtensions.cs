using System;

namespace Gsemac.Text.Ini.Extensions {

    public static class IniSectionExtensions {

        // Public members

        public static IIniProperty Get(this IIniSection section, string name) {

            if (section is null)
                throw new ArgumentNullException(nameof(section));

            return section.Properties.Get(name);

        }

        public static string GetValue(this IIniSection section, string name) {

            if (section is null)
                throw new ArgumentNullException(nameof(section));

            return section.Properties.Get(name)?.Value ?? String.Empty;

        }
        public static void SetValue(this IIniSection section, string name, string value) {

            if (section is null)
                throw new ArgumentNullException(nameof(section));

            section.Properties.SetValue(name, value);

        }

    }

}