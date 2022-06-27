using System;
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
        public IniSection(IEqualityComparer<string> keyComparer) :
           this(string.Empty, keyComparer) {
        }
        public IniSection(string name) :
            this(name, EqualityComparer<string>.Default) {
        }
        public IniSection(string name, IEqualityComparer<string> keyComparer) {

            if (keyComparer is null)
                throw new ArgumentNullException(nameof(keyComparer));

            Name = name;

            properties = new IniPropertyCollection(keyComparer);
            sections = new IniSectionCollection(keyComparer);

        }

        public IEnumerator<IIniProperty> GetEnumerator() {

            return properties.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IIniPropertyCollection properties;
        private readonly IIniSectionCollection sections;

    }

}