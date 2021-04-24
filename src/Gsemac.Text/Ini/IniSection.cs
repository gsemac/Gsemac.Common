using Gsemac.Collections;
using System;
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
        public IniSection(string name, IIniDocument parentIniData) :
            this(name) {

            this.parentIniData = parentIniData;

        }

        public void AddProperty(IIniProperty property) {

            if (property is null)
                throw new ArgumentNullException(nameof(property));

            properties[GetKey(property.Name)] = property;

            // Add the section to its parent if we haven't done so already.
            // This is so temporary sections can be created that are only added when values are added.

            if (!(parentIniData is null) && !addedToParent) {

                parentIniData.AddSection(this);

                addedToParent = true;

            }

        }
        public IIniProperty GetProperty(string name) {

            if (properties.TryGetValue(GetKey(name), out IIniProperty property))
                return property;

            return null;

        }
        public bool RemoveProperty(string name) {

            return properties.Remove(GetKey(name));

        }

        public IEnumerator<IIniProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDictionary<string, IIniProperty> properties = new OrderedDictionary<string, IIniProperty>();
        private readonly IIniDocument parentIniData = null;
        private bool addedToParent = false;

        private string GetKey(string propertyName) {

            if (string.IsNullOrEmpty(propertyName))
                return propertyName;

            return propertyName.ToLowerInvariant().Trim();

        }

    }

}