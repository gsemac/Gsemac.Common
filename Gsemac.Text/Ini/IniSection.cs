using Gsemac.Collections;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public class IniSection :
        IEnumerable<IniProperty> {

        // Public members

        public string this[string key] {
            get => GetProperty(key)?.Value ?? string.Empty;
            set => AddProperty(key, value);
        }

        public string Name { get; }

        public IniSection(string name) {

            Name = name;

        }

        public void AddProperty(string name, string value) {

            AddProperty(new IniProperty(name, value));

        }
        public void AddProperty(IniProperty property) {

            properties[property.Name.ToLowerInvariant()] = property;

        }
        public IniProperty GetProperty(string name) {

            if (properties.TryGetValue(name.ToLowerInvariant(), out IniProperty property))
                return property;

            return null;

        }
        public bool RemoveProperty(string name) {

            return properties.Remove(name.ToLowerInvariant());

        }

        public IEnumerator<IniProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDictionary<string, IniProperty> properties = new OrderedDictionary<string, IniProperty>();

    }

}