using System.Linq;

namespace Gsemac.Text.Ini.Extensions {

    public static class IniDataExtensions {

        public static void AddSection(this IIniData iniData, string name) {

            iniData.AddSection(new IniSection(name));

        }
        public static bool HasSection(this IIniData iniData, string name) {

            return !(iniData.GetSection(name) is null);

        }

        public static bool TryGetValue<T>(this IIniData iniData, string propertyName, out T propertyValue) {

            foreach (IIniSection section in new[] { iniData.DefaultSection }.Concat(iniData)) {

                if (section.TryGetValue(propertyName, out propertyValue))
                    return true;

            }

            propertyValue = default;

            return false;

        }
        public static bool TryGetValue<T>(this IIniData iniData, string sectionName, string propertyName, out T propertyValue) {

            IIniSection section = iniData.GetSection(sectionName);

            if (!(section is null))
                return section.TryGetValue(propertyName, out propertyValue);

            propertyValue = default;

            return false;

        }
        public static string GetValue(this IIniData iniData, string propertyName) {

            return iniData.GetValue<string>(propertyName);

        }
        public static string GetValue(this IIniData iniData, string sectionName, string propertyName) {

            return iniData.GetValue<string>(sectionName, propertyName);

        }
        public static T GetValue<T>(this IIniData iniData, string propertyName) {

            iniData.TryGetValue(propertyName, out T propertyValue);

            return propertyValue;

        }
        public static T GetValue<T>(this IIniData iniData, string sectionName, string propertyName) {

            iniData.TryGetValue(sectionName, propertyName, out T propertyValue);

            return propertyValue;

        }
        public static void SetValue<T>(this IIniData iniData, string propertyName, T propertyValue) {

            iniData.DefaultSection.SetValue(propertyName, propertyValue);

        }
        public static void SetValue<T>(this IIniData iniData, string sectionName, string propertyName, T propertyValue) {

            iniData[sectionName].SetValue(propertyName, propertyValue);

        }

    }

}