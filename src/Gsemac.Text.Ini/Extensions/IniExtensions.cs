using System;
using System.IO;

namespace Gsemac.Text.Ini.Extensions {

    public static class IniExtensions {

        // Public members

        public static IIniProperty Get(this IIni ini, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            return ini.Global.Properties.Get(propertyName);

        }
        public static IIniProperty Get(this IIni ini, string sectionName, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            return ini.Sections.Get(sectionName).Properties.Get(propertyName);

        }

        public static string GetValue(this IIni ini, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            return ini.Global.Properties.Get(propertyName)?
                .Value ?? string.Empty;

        }
        public static string GetValue(this IIni ini, string sectionName, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            return ini.Sections.Get(sectionName)?
                    .Properties.Get(propertyName)?
                    .Value ?? string.Empty;

        }

        public static void SetValue(this IIni ini, string propertyName, string propertyValue) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            ini.Global.Properties.Add(propertyName, propertyValue);

        }
        public static void SetValue(this IIni ini, string sectionName, string propertyName, string propertyValue) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            IIniSection section = GetOrAddSection(ini, sectionName);

            section.Properties.Set(propertyName, propertyValue);

        }

        public static void Add(this IIni ini, string propertyName, string propertyValue) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            ini.Global.Properties.Add(propertyName, propertyValue);

        }
        public static void Add(this IIni ini, string sectionName, string propertyName, string propertyValue) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            IIniSection section = GetOrAddSection(ini, sectionName);

            section.Properties.Add(propertyName, propertyValue);

        }

        public static bool Remove(this IIni ini, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            return ini.Global.Properties.Remove(propertyName);

        }
        public static bool Remove(this IIni ini, string sectionName, string propertyName) {

            if (ini is null)
                throw new ArgumentNullException(nameof(ini));

            IIniSection section = ini.Sections.Get(sectionName);

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

            IIniSection section = ini.Sections.Get(sectionName);

            if (section is null)
                return false;

            return section.Properties.Contains(propertyName);

        }

        public static void Save(this IIni ini, string filePath) {

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                Save(ini, fs);

        }
        public static void Save(this IIni ini, Stream stream) {

            using (StreamWriter sw = new StreamWriter(stream))
                sw.Write(ini.ToString());

        }

        // Private members

        private static IIniSection GetOrAddSection(IIni ini, string sectionName) {

            IIniSection section = ini.Sections.Get(sectionName);

            if (section is null) {

                ini.Sections.Add(sectionName);

                section = ini.Sections.Get(sectionName);

            }

            return section;

        }

    }

}