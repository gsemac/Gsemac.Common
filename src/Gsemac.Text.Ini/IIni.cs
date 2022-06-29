using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIni :
        IEnumerable<IIniSection> {

        IIniSection this[string sectionName] { get; }

        IIniSection Global { get; }
        IIniSectionCollection Sections { get; }

        string GetValue(string propertyName);
        string GetValue(string sectionName, string propertyName);

        T GetValue<T>(string propertyName);
        T GetValue<T>(string sectionName, string propertyName);

        bool TryGetValue(string propertyName, out string value);
        bool TryGetValue(string sectionName, string propertyName, out string value);

        bool TryGetValue<T>(string propertyName, out T value);
        bool TryGetValue<T>(string sectionName, string propertyName, out T value);

        void SetValue(string propertyName, string value);
        void SetValue(string sectionName, string propertyName, string value);

        void SetValue<T>(string propertyName, T value);
        void SetValue<T>(string sectionName, string propertyName, T value);

    }

}