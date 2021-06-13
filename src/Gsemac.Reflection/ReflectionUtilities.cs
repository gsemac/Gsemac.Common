using System;
using System.Reflection;

namespace Gsemac.Reflection {

    public static class ReflectionUtilities {

        public static void CopyProperties(object fromObject, object toObject) {

            CopyProperties(fromObject, toObject, CopyPropertiesOptions.Default);

        }
        public static void CopyProperties(object fromObject, object toObject, ICopyPropertiesOptions options) {

            if (fromObject is null)
                throw new ArgumentNullException(nameof(fromObject));

            if (toObject is null)
                throw new ArgumentNullException(nameof(toObject));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            Type sourceType = fromObject.GetType();
            Type destinationType = toObject.GetType();

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            if (options.CopyNonPublicProperties)
                bindingFlags |= BindingFlags.NonPublic;

            foreach (PropertyInfo sourceProperty in sourceType.GetProperties(bindingFlags)) {

                if (!sourceProperty.CanRead)
                    continue;

                PropertyInfo destinationProperty = destinationType.GetProperty(sourceProperty.Name, bindingFlags);

                // Some of the following set of conditions was taken from https://stackoverflow.com/a/8724150/5383169 (Azerothian)

                if (destinationProperty is null || !destinationProperty.CanWrite)
                    continue;

                MethodInfo destinationPropertySetMethod = destinationProperty.GetSetMethod(nonPublic: true);

                if (destinationPropertySetMethod is object) {

                    if (destinationPropertySetMethod.IsPrivate)
                        continue;

                    if (destinationPropertySetMethod.Attributes.HasFlag(MethodAttributes.Static))
                        continue;

                }

                if (!destinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    continue;

                try {

                    object sourceValue = sourceProperty.GetValue(fromObject, null);

                    if (sourceValue is object || options.CopyNullProperties)
                        destinationProperty.SetValue(toObject, sourceValue, null);

                }
                catch (Exception) {

                    if (!options.IgnoreExceptions)
                        throw;

                }

            }

        }

    }

}