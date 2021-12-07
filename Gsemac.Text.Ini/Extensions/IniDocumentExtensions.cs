using System.IO;
using System.Linq;

namespace Gsemac.Text.Ini.Extensions {

    public static class IniDocumentExtensions {

        public static void AddSection(this IIniDocument document, string name) {

            document.AddSection(new IniSection(name));

        }
        public static bool HasSection(this IIniDocument document, string name) {

            return !(document.GetSection(name) is null);

        }

        public static bool TryGetValue<T>(this IIniDocument document, string propertyName, out T propertyValue) {

            foreach (IIniSection section in new[] { document.DefaultSection }.Concat(document)) {

                if (section.TryGetValue(propertyName, out propertyValue))
                    return true;

            }

            propertyValue = default;

            return false;

        }
        public static bool TryGetValue<T>(this IIniDocument document, string sectionName, string propertyName, out T propertyValue) {

            IIniSection section = document.GetSection(sectionName);

            if (!(section is null))
                return section.TryGetValue(propertyName, out propertyValue);

            propertyValue = default;

            return false;

        }
        public static string GetValue(this IIniDocument document, string propertyName) {

            return document.GetValue<string>(propertyName);

        }
        public static string GetValue(this IIniDocument document, string sectionName, string propertyName) {

            return document.GetValue<string>(sectionName, propertyName);

        }
        public static T GetValue<T>(this IIniDocument document, string propertyName) {

            document.TryGetValue(propertyName, out T propertyValue);

            return propertyValue;

        }
        public static T GetValue<T>(this IIniDocument document, string sectionName, string propertyName) {

            document.TryGetValue(sectionName, propertyName, out T propertyValue);

            return propertyValue;

        }
        public static void SetValue<T>(this IIniDocument document, string propertyName, T propertyValue) {

            document.DefaultSection.SetValue(propertyName, propertyValue);

        }
        public static void SetValue<T>(this IIniDocument document, string sectionName, string propertyName, T propertyValue) {

            document[sectionName].SetValue(propertyName, propertyValue);

        }

        public static void Save(this IIniDocument document, string filePath) {

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                Save(document, fs);

        }
        public static void Save(this IIniDocument document, Stream stream) {

            using (StreamWriter sw = new StreamWriter(stream))
                sw.Write(document.ToString());

        }

    }

}