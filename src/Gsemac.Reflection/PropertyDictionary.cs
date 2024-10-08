﻿using Gsemac.Reflection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.Reflection {

    public sealed class PropertyDictionary :
        IPropertyDictionary {

        // Public members

        public object this[string key] {
            get => GetPropertyValue(key);
            set => SetPropertyValue(key, value);
        }

        public ICollection<string> Keys => GetPropertyKeys();
        public ICollection<object> Values => GetKeyValuePairs().Select(pair => pair.Value).ToList().AsReadOnly();
        public int Count => Keys.Count;
        public bool IsReadOnly => false;

        public PropertyDictionary(object obj) :
            this(obj, PropertyDictionaryOptions.Default) {
        }
        public PropertyDictionary(object obj, IPropertyDictionaryOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            baseObject = obj;
            this.options = options;

        }

        public bool Contains(KeyValuePair<string, object> item) {

            if (TryGetValue(item.Key, out object value))
                return (value is null && item.Value is null) || value.Equals(item.Value);

            return false;

        }
        public bool ContainsKey(string key) {

            lock (propertyCacheMutex) {

                RebuildPropertyCacheForKey(key);

                return propertyCache.TryGetValue(key, out _);

            }

        }
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {

            GetKeyValuePairs().ToArray().CopyTo(array, arrayIndex);

        }
        public bool TryGetValue(string key, out object value) {

            if (key is null)
                throw new ArgumentNullException(nameof(key));

            lock (propertyCacheMutex) {

                RebuildPropertyCacheForKey(key);

                if (propertyCache.TryGetValue(key, out var objectProperty) && objectProperty.Object is object) {

                    value = GetPropertyValue(objectProperty);

                    return true;

                }
                else {

                    value = null;

                    return false;

                }

            }

        }
        public bool TryGetPropertyInfo(string key, out PropertyInfo value) {

            if (key is null)
                throw new ArgumentNullException(nameof(key));

            lock (propertyCacheMutex) {

                RebuildPropertyCacheForKey(key);

                if (propertyCache.TryGetValue(key, out var objectProperty) && objectProperty.Object is object) {

                    value = objectProperty.Property;

                    return true;

                }
                else {

                    value = null;

                    return false;

                }

            }

        }
        public bool TryGetValue<T>(string key, out T value) {

            value = default;

            if (TryGetValue(key, out object obj)) {

                if (obj is T tValue) {

                    value = tValue;

                    return true;

                }
                else if (TypeUtilities.TryConvert(obj, out T castedValue)) {

                    value = castedValue;

                    return true;

                }

            }

            return false;

        }
        public bool TrySetValue<T>(string key, T value) {

            lock (propertyCacheMutex) {

                RebuildPropertyCacheForKey(key);

                if (propertyCache.TryGetValue(key, out var objectProperty) && objectProperty.Object is object) {

                    Type propertyType = objectProperty.Property.PropertyType;

                    if (propertyType.IsAssignableFrom(typeof(T))) {

                        SetPropertyValue(objectProperty, value);

                        return true;

                    }
                    else if (TypeUtilities.TryConvert(value, propertyType, out object castedValue)) {

                        SetPropertyValue(objectProperty, castedValue);

                        return true;

                    }

                }

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

        private readonly object baseObject;
        private readonly IPropertyDictionaryOptions options;
        private readonly object propertyCacheMutex = new object();
        private IDictionary<string, ObjectPropertyPair> propertyCache;
        private int propertyCacheDepth = 0;

        private void RebuildPropertyCache(int maxDepth = int.MaxValue) {

            lock (propertyCacheMutex) {

                if (propertyCacheDepth < maxDepth) {

                    propertyCache = CreatePropertyCache(baseObject, baseObject.GetType(), maxDepth, options);
                    propertyCacheDepth = maxDepth;

                }

            }

        }
        private void RebuildPropertyCacheForKey(string key) {

            // Make sure the property cache is deep enough to find a property with the given level of nesting.

            if (!string.IsNullOrEmpty(key))
                RebuildPropertyCache(key.Count(c => c == '.') + 1);

        }

        private ICollection<string> GetPropertyKeys() {

            // Build the entire property cache so we have access to all keys.

            RebuildPropertyCache();

            lock (propertyCacheMutex)
                return propertyCache.Keys;

        }
        private IEnumerable<KeyValuePair<string, object>> GetKeyValuePairs() {

            return GetPropertyKeys().Select(key => new KeyValuePair<string, object>(key, GetPropertyValue(key)));

        }

        private static IDictionary<string, ObjectPropertyPair> CreatePropertyCache(object obj, Type type, int maxDepth, IPropertyDictionaryOptions options) {

            IDictionary<string, ObjectPropertyPair> propertyCache = new Dictionary<string, ObjectPropertyPair>(options.StringComparer ?? StringComparer.Ordinal);

            if (maxDepth <= 0 || obj is null)
                return propertyCache;

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

            foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags)) {

                string propertyKey = propertyInfo.Name;
                Type propertyType = propertyInfo.PropertyType;

                bool propertyTypeIsObject = !propertyType.IsBuiltIn();

                if (propertyInfo.CanWrite || !options.SkipReadOnlyProperties)
                    propertyCache[propertyKey] = new ObjectPropertyPair(obj, propertyInfo);

                if (propertyTypeIsObject && options.IncludeNestedProperties) {

                    var childPropertyDict = CreatePropertyCache(obj is null ? null : propertyInfo.GetValue(obj, null), propertyType, maxDepth - 1, options);

                    foreach (string key in childPropertyDict.Keys)
                        propertyCache[$"{propertyKey}.{key}"] = childPropertyDict[key];

                }

            }

            return propertyCache;

        }

        private object GetPropertyValue(string key) {

            lock (propertyCacheMutex) {

                RebuildPropertyCacheForKey(key);

                var objectProperty = propertyCache[key];

                return GetPropertyValue(objectProperty);

            }

        }
        private void SetPropertyValue(string key, object value) {

            lock (propertyCacheMutex) {

                RebuildPropertyCacheForKey(key);

                var objectProperty = propertyCache[key];

                SetPropertyValue(objectProperty, value);

            }

        }
        private object GetPropertyValue(ObjectPropertyPair objectProperty) {

            lock (propertyCacheMutex)
                return objectProperty.Value;

        }
        private void SetPropertyValue(ObjectPropertyPair objectProperty, object value) {

            // Replace all instances of the old object in the dictionary with the newly-assigned object.
            // This makes sure gets/sets on nested properties are done on the right object.

            lock (propertyCacheMutex) {

                if (!objectProperty.Property.PropertyType.IsBuiltIn()) {

                    object oldPropertyValue = GetPropertyValue(objectProperty);

                    foreach (ObjectPropertyPair pair in propertyCache.Values.Where(p => ReferenceEquals(p.Object, oldPropertyValue)))
                        pair.Object = value;

                }

                objectProperty.Value = value;

            }

        }

    }

}