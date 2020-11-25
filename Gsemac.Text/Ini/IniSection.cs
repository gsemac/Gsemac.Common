using Gsemac.Collections;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public class IniSection :
        IIniSection {

        // Public members

        public string this[string key] {
            get => GetProperty(key)?.Value ?? string.Empty;
            set => AddProperty(new IniProperty(key, value));
        }

        public string Name { get; }

        public IniSection(string name) {

            Name = name;

        }

        public void AddProperty(IIniProperty property) {

            properties[property.Name.ToLowerInvariant()] = property;

        }
        public IIniProperty GetProperty(string name) {

            if (properties.TryGetValue(name.ToLowerInvariant(), out IIniProperty property))
                return property;

            return null;

        }
        public bool RemoveProperty(string name) {

            return properties.Remove(name.ToLowerInvariant());

        }

        public IEnumerator<IIniProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDictionary<string, IIniProperty> properties = new OrderedDictionary<string, IIniProperty>();

    }

}