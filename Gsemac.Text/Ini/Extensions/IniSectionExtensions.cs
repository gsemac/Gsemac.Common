namespace Gsemac.Text.Ini.Extensions {

    public static class IniSectionExtensions {

        public static void AddProperty(this IIniSection iniSection, string name, string value) {

            iniSection.AddProperty(new IniProperty(name, value));

        }

    }

}