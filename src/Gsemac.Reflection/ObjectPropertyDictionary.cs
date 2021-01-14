using Gsemac.Polyfills.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.Reflection {

    public class ObjectPropertyDictionary :
        IObjectPropertyDictionary {

        // Public members

        public object this[string key] {
            get => GetPropertyValue(key);
            set => SetPropertyValue(key, value);
        }

        public ICollection<string> Keys => propertyDict.Keys;
        public ICollection<object> Values => propertyDict.Values.Select(objectProperty => GetPropertyValue(objectProperty)).ToList().AsReadOnly();
        public int Count => propertyDict.Keys.Count;
        public bool IsReadOnly => propertyDict.IsReadOnly;

        public ObjectPropertyDictionary(object obj) {

            propertyDict = BuildPropertyDict(obj, obj.GetType());

        }

        public bool Contains(KeyValuePair<string, object> item) {

            return GetKeyValuePairs().Any(pair => pair.Key.Equals(item.Key) && pair.Value.Equals(item.Value));

        }
        public bool ContainsKey(string key) {

            return propertyDict.ContainsKey(key);

        }
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {

            GetKeyValuePairs().ToArray()
                .CopyTo(array, arrayIndex);

        }
        public bool TryGetValue(string key, out object value) {

            if (propertyDict.TryGetValue(key, out var objectProperty)) {

                value = GetPropertyValue(objectProperty);

                return true;

            }
            else {

                value = null;

                return false;

            }

        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {

            return GetKeyValuePairs().GetEnumerator();

        }

        void IDictionary<string, object>.Add(string key, object value) {

            throw new NotSupportedException();

        }
        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item) {

            throw new NotSupportedException();

        }
        bool IDictionary<string, object>.Remove(string key) {

            throw new NotSupportedException();

        }
        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) {

            throw new NotSupportedException();

        }
        void ICollection<KeyValuePair<string, object>>.Clear() {

            throw new NotSupportedException();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IDictionary<string, Tuple<object, PropertyInfo>> propertyDict;

        private IDictionary<string, Tuple<object, PropertyInfo>> BuildPropertyDict(object obj, Type type) {

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

            var propertyDict = new Dictionary<string, Tuple<object, PropertyInfo>>();

            foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags)) {

                string propertyKey = propertyInfo.Name;

                // Check if the property is a primitive type.
                // https://stackoverflow.com/a/2664428 (Michael Petito)

                if (Type.GetTypeCode(propertyInfo.PropertyType) == TypeCode.Object) {

                    var childPropertyDict = BuildPropertyDict(propertyInfo.GetValue(obj), propertyInfo.PropertyType);

                    foreach (string key in childPropertyDict.Keys)
                        propertyDict[$"{propertyKey}.{key}"] = childPropertyDict[key];

                }
                else {

                    propertyDict[propertyKey] = Tuple.Create(obj, propertyInfo);

                }

            }

            return propertyDict;

        }
        private void SetPropertyValue(string key, object value) {

            var objectProperty = propertyDict[key];

            objectProperty.Item2.SetValue(objectProperty.Item1, value);

        }
        private object GetPropertyValue(Tuple<object, PropertyInfo> objectProperty) {

            return objectProperty.Item2.GetValue(objectProperty.Item1);

        }
        private object GetPropertyValue(string key) {

            var objectProperty = propertyDict[key];

            return GetPropertyValue(objectProperty);

        }
        private IEnumerable<KeyValuePair<string, object>> GetKeyValuePairs() {

            return propertyDict.Select(pair => new KeyValuePair<string, object>(pair.Key, GetPropertyValue(pair.Key)));

        }

    }

}