using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public class IniPropertyCollection :
        IIniPropertyCollection {

        // Public members

        public int Count => properties.Count;
        public bool IsReadOnly => properties.IsReadOnly;

        public IIniProperty this[string propertyName] {
            get => Get(propertyName);
        }

        public void Add(string propertyName, string propertyValue) {

            properties.Add(propertyName, new IniProperty(propertyName, propertyValue));

        }
        public void Add(IIniProperty item) {

            properties.Add(item.Name, item);

        }

        public void Clear() {

            properties.Clear();

        }

        public bool Contains(IIniProperty item) {

            if (item is null)
                return false;

            return properties.TryGetValue(item.Name, out IIniProperty property) &&
                property.Equals(item);

        }
        public bool Contains(string propertyName) {

            return properties.ContainsKey(propertyName);

        }

        public void CopyTo(IIniProperty[] array, int arrayIndex) {

            properties.Values.CopyTo(array, arrayIndex);

        }

        public IIniProperty Get(string propertyName) {

            if (properties.TryGetValue(propertyName, out IIniProperty property))
                return property;

            return null;

        }
        public void Set(string propertyName, string propertyValue) {

            if (properties.TryGetValue(propertyName, out IIniProperty property))
                property.Value = propertyValue;

            Add(propertyName, propertyValue);

        }

        public bool Remove(string propertyName) {

            return properties.Remove(propertyName);

        }
        public bool Remove(IIniProperty item) {

            if (item is null)
                return false;

            // If we have an identical property to the one given, remove it.

            if (Contains(item))
                return properties.Remove(item.Name);

            return false;

        }

        public IEnumerator<IIniProperty> GetEnumerator() {

            return properties.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDictionary<string, IIniProperty> properties = new Dictionary<string, IIniProperty>();

    }

}