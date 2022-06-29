using System;
using System.IO;

namespace Gsemac.Text.Ini {

    public static class IniExtensions {

        // Public members

        public static string GetValueOrDefault(this IIni ini, string propertyName, string defaultValue) {

            if (ini.TryGetValue(propertyName, out string value))
                return value;

            return defaultValue;

        }
        public static string GetValueOrDefault(this IIni ini, string sectionName, string propertyName, string defaultValue) {

            if (ini.TryGetValue(sectionName, propertyName, out string value))
                return value;

            return defaultValue;

        }

        public static T GetValueOrDefault<T>(this IIni ini, string propertyName, T defaultValue) {

            if (ini.TryGetValue(propertyName, out T value))
                return value;

            return defaultValue;

        }
        public static T GetValueOrDefault<T>(this IIni ini, string sectionName, string propertyName, T defaultValue) {

            if (ini.TryGetValue(sectionName, propertyName, out T value))
                return value;

            return defaultValue;

        }

        public static bool Remove(this IIni ini, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            return ini.Global.Properties.Remove(propertyName);

        }
        public static bool Remove(this IIni ini, string sectionName, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            IIniSection section = ini.Sections[sectionName];

            if (section is null)
                return false;

            return section.Properties.Remove(propertyName);

        }

        public static bool Contains(this IIni ini, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            return ini.Global.Properties.Contains(propertyName);

        }
        public static bool Contains(this IIni ini, string sectionName, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            return ini[sectionName].Properties.Contains(propertyName);

        }

        public static void Save(this IIni ini, string filePath) {

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                ini.Save(fs);

        }
        public static void Save(this IIni ini, Stream stream) {

            using (StreamWriter sw = new StreamWriter(stream))
                sw.Write(ini.ToString());

        }

    }

}