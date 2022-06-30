using System;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public class IniPropertyCollection :
        IIniPropertyCollection {

        // Public members

        public int Count => properties.Count;
        public bool IsReadOnly => properties.IsReadOnly;

        public IIniProperty this[string propertyName] {
            get => Get(FormatPropertyName(propertyName));
        }

        public IniPropertyCollection() :
            this(StringComparer.InvariantCultureIgnoreCase) {
        }
        public IniPropertyCollection(IEqualityComparer<string> keyComparer) {

            if (keyComparer is null)
                throw new ArgumentNullException(nameof(keyComparer));

            properties = new Dictionary<string, IIniProperty>(keyComparer);

        }

        public void Add(string name, string value) {

            Add(new IniProperty(FormatPropertyName(name), value));

        }
        public void Add(IIniProperty item) {

            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (properties.TryGetValue(item.Name, out IIniProperty existingProperty)) {

                // Update the existing property.

                existingProperty.Value = item.Value;

            }
            else {

                properties.Add(item.Name, item);

            }

        }

        public bool Remove(string name) {

            return properties.Remove(name);

        }
        public bool Remove(IIniProperty item) {

            if (item is null)
                return false;

            // If we have an identical property to the one given, remove it.

            if (Contains(item))
                return properties.Remove(item.Name);

            return false;

        }

        public bool Contains(IIniProperty item) {

            if (item is null)
                return false;

            return properties.TryGetValue(item.Name, out IIniProperty property) &&
                property.Equals(item);

        }
        public bool Contains(string name) {

            return properties.ContainsKey(FormatPropertyName(name));

        }

        public void Clear() {

            properties.Clear();

        }

        public void CopyTo(IIniProperty[] array, int arrayIndex) {

            properties.Values.CopyTo(array, arrayIndex);

        }

        public IEnumerator<IIniProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDictionary<string, IIniProperty> properties;

        private string FormatPropertyName(string value) {

            if (value is null)
                value = string.Empty;

            return value;

        }

        private IIniProperty Get(string name) {

            if (name is object && properties.TryGetValue(name, out IIniProperty property))
                return property;

            return null;

        }

    }

}