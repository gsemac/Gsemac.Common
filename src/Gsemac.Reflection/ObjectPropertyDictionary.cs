using Gsemac.Reflection.Extensions;
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
        public ICollection<object> Values => propertyDict.Values.Select(p => GetPropertyValue(p)).ToList().AsReadOnly();
        public int Count => propertyDict.Keys.Count;
        public bool IsReadOnly => propertyDict.IsReadOnly;

        public ObjectPropertyDictionary(object obj, ObjectPropertyDictionaryOptions options = ObjectPropertyDictionaryOptions.Default) {

            propertyDict = BuildPropertyDict(obj, obj.GetType(), options);

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
        public bool TryGetValue<T>(string key, out T value) {

            value = default;

            if (propertyDict.TryGetValue(key, out var objectProperty) && objectProperty.Object is object) {

                object objValue = GetPropertyValue(objectProperty);

                if (objValue is T tValue) {

                    value = tValue;

                    return true;

                }
                else if (objValue.TryCast(out T castedValue)) {

                    value = castedValue;

                    return true;

                }

            }

            return false;

        }
        public bool TrySetValue<T>(string key, T value) {

            if (propertyDict.TryGetValue(key, out var objectProperty) && objectProperty.Object is object) {

                Type propertyType = objectProperty.Property.PropertyType;

                if (propertyType.IsAssignableFrom(typeof(T))) {

                    SetPropertyValue(objectProperty, value);

                    return true;

                }
                else if (value.TryCast(propertyType, out object castedValue)) {

                    SetPropertyValue(objectProperty, castedValue);

                    return true;

                }

            }

            return false;

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

        private class ObjectPropertyPair {

            // Public members

            public object Object { get; set; }
            public PropertyInfo Property { get; }
            public object Value {
                get => Property.GetValue(Object, null);
                set => Property.SetValue(Object, value, null);
            }

            public ObjectPropertyPair(object @object, PropertyInfo @property) {

                Object = @object;
                Property = @property;

            }

        }

        private readonly IDictionary<string, ObjectPropertyPair> propertyDict;
        private readonly object objectUpdateMutex = new object();

        private IDictionary<string, ObjectPropertyPair> BuildPropertyDict(object obj, Type type, ObjectPropertyDictionaryOptions options) {

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

            var propertyDict = new Dictionary<string, ObjectPropertyPair>();

            foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags)) {

                string propertyKey = propertyInfo.Name;
                bool propertyTypeIsObject = Type.GetTypeCode(propertyInfo.PropertyType) == TypeCode.Object;

                if (propertyInfo.CanWrite || !options.HasFlag(ObjectPropertyDictionaryOptions.SkipReadOnlyProperties))
                    propertyDict[propertyKey] = new ObjectPropertyPair(obj, propertyInfo);

                // Check if the property is a primitive type.
                // https://stackoverflow.com/a/2664428 (Michael Petito)

                if (propertyTypeIsObject && options.HasFlag(ObjectPropertyDictionaryOptions.IncludeNestedProperties)) {

                    var childPropertyDict = BuildPropertyDict(propertyInfo.GetValue(obj, null), propertyInfo.PropertyType, options);

                    foreach (string key in childPropertyDict.Keys)
                        propertyDict[$"{propertyKey}.{key}"] = childPropertyDict[key];

                }

            }

            return propertyDict;

        }


        private object GetPropertyValue(string key) {

            var objectProperty = propertyDict[key];

            return GetPropertyValue(objectProperty);

        }
        private void SetPropertyValue(string key, object value) {

            var objectProperty = propertyDict[key];

            SetPropertyValue(objectProperty, value);

        }
        private object GetPropertyValue(ObjectPropertyPair objectProperty) {

            lock (objectUpdateMutex)
                return objectProperty.Value;

        }
        private void SetPropertyValue(ObjectPropertyPair objectProperty, object value) {

            // Replace all instances of the old object in the dictionary with the newly-assigned object.
            // This makes sure gets/sets on nested properties are done on the right object.

            lock (objectUpdateMutex) {

                if (Type.GetTypeCode(objectProperty.Property.PropertyType) == TypeCode.Object) {

                    object oldPropertyValue = GetPropertyValue(objectProperty);

                    foreach (ObjectPropertyPair pair in propertyDict.Values.Where(p => ReferenceEquals(p.Object, oldPropertyValue)))
                        pair.Object = value;

                }

                objectProperty.Value = value;

            }

        }

        private IEnumerable<KeyValuePair<string, object>> GetKeyValuePairs() {

            return propertyDict.Select(pair => new KeyValuePair<string, object>(pair.Key, GetPropertyValue(pair.Key)));

        }

    }

}