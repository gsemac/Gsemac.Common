using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public class IniSection :
        IIniSection {

        // Public members

        public string this[string propertyName] {
            get => properties.Get(propertyName)?.Value ?? string.Empty;
            set => properties.Add(propertyName, value);
        }

        public string Name { get; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public IIniPropertyCollection Properties => properties;
        public IIniSectionCollection Sections => sections;

        public int Count => properties.Count;
        public bool IsReadOnly => properties.IsReadOnly;

        public IniSection() :
            this(string.Empty) {
        }
        public IniSection(string name) {

            Name = name;

        }

        public IEnumerator<IIniProperty> GetEnumerator() {

            return properties.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IIniPropertyCollection properties = new IniPropertyCollection();
        private readonly IIniSectionCollection sections = new IniSectionCollection();

    }

}