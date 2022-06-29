using Gsemac.Data.ValueConversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Text.Ini {

    public abstract class IniBase :
        IIni {

        // Public members

        public IIniSection this[string sectionName] {
            get => Sections[sectionName];
        }

        public abstract IIniSection Global { get; }
        public abstract IIniSectionCollection Sections { get; }

        public string GetValue(string propertyName) {

            return Global
                .Properties[propertyName]?
                .Value ?? string.Empty;

        }
        public string GetValue(string sectionName, string propertyName) {

            return Sections[sectionName]?
                .Properties[propertyName]?
                .Value ?? string.Empty;

        }

        public T GetValue<T>(string propertyName) {

            string value = GetValue(propertyName);

            return (T)options.ValueConverterFactory
                .Create<string, T>()
                .Convert(value);

        }
        public T GetValue<T>(string sectionName, string propertyName) {

            string value = GetValue(sectionName, propertyName);

            return (T)options.ValueConverterFactory
                .Create<string, T>()
                .Convert(value);

        }

        public bool TryGetValue(string propertyName, out string value) {

            value = string.Empty;

            IIniProperty property = Global.Properties[propertyName];

            if (property is object) {

                value = property.Value;

                return true;

            }

            return false;

        }
        public bool TryGetValue(string sectionName, string propertyName, out string value) {

            value = string.Empty;

            IIniProperty property = this[sectionName]?.Properties[propertyName];

            if (property is object) {

                value = property.Value;

                return true;

            }

            return false;

        }

        public bool TryGetValue<T>(string propertyName, out T value) {

            value = default;

            if (TryGetValue(propertyName, out string stringValue) &&
                options.ValueConverterFactory.Create<string, T>().TryConvert(stringValue, out object convertedValue)) {

                value = (T)convertedValue;

                return true;

            }

            return false;

        }
        public bool TryGetValue<T>(string sectionName, string propertyName, out T value) {

            value = default;

            if (TryGetValue(sectionName, propertyName, out string stringValue) &&
                options.ValueConverterFactory.Create<string, T>().TryConvert(stringValue, out object convertedValue)) {

                value = (T)convertedValue;

                return true;

            }

            return false;

        }

        public void SetValue(string propertyName, string value) {

            Global.Properties.Add(propertyName, value);

        }
        public void SetValue(string sectionName, string propertyName, string value) {

            IIniSection section = GetOrAddSection(sectionName);

            section.Properties.Add(propertyName, value);

        }

        public void SetValue<T>(string propertyName, T value) {

            if (options.ValueConverterFactory.Create<T, string>().TryConvert(value, out object stringValue)) {

                SetValue(propertyName, (string)stringValue);

            }
            else {

                SetValue(propertyName, value.ToString());

            }

        }
        public void SetValue<T>(string sectionName, string propertyName, T value) {

            if (options.ValueConverterFactory.Create<T, string>().TryConvert(value, out object stringValue)) {

                SetValue(sectionName, propertyName, (string)stringValue);

            }
            else {

                SetValue(sectionName, propertyName, value.ToString());

            }

        }

        public IEnumerator<IIniSection> GetEnumerator() {

            return Sections.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            using (IIniWriter writer = new IniWriter(options)) {

                if (Global.Any()) {

                    WriteSection(writer, Global);

                }

                foreach (IIniSection section in this) {

                    if (section == Global)
                        continue;

                    WriteSection(writer, section);

                }

                return writer.ToString();

            }

        }

        // Protected members

        protected IniBase() :
            this(IniOptions.Default) {
        }
        protected IniBase(IIniOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        // Private members

        private readonly IIniOptions options;

        private void WriteSection(IIniWriter writer, IIniSection section) {

            if (section != Global && !string.IsNullOrEmpty(section.Name)) {

                writer.WriteSectionStart();
                writer.WriteSectionName(section.Name);
                writer.WriteSectionEnd();

            }

            foreach (IIniProperty property in section) {

                if (options.EnableComments && !string.IsNullOrEmpty(options.CommentMarker) && !string.IsNullOrEmpty(property.Comment))
                    writer.WriteComment(property.Comment);

                writer.WritePropertyName(property.Name);
                writer.WriteNameValueSeparator();
                writer.WritePropertyValue(property.Value);

            }

        }

        private IIniSection GetOrAddSection(string sectionName) {

            IIniSection section = Sections[sectionName];

            if (section is null) {

                Sections.Add(sectionName);

                section = Sections[sectionName];

            }

            return section;

        }

    }

}