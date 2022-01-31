using Gsemac.Reflection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.Reflection {

    public static class ReflectionUtilities {

        // Public members

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

                // Ensure that the destination property exists and that we are able to write to it.
                // Some of the following conditions were taken from https://stackoverflow.com/a/8724150/5383169 (Azerothian)

                if (destinationProperty is null)
                    continue;

                bool copyCollectionItems = options.CopyCollectionItems &&
                    destinationProperty.CanRead &&
                    sourceProperty.PropertyType.ImplementsInterface<IEnumerable>() &&
                    destinationProperty.PropertyType.ImplementsInterface<ICollection>();

                try {

                    if (copyCollectionItems) {

                        // Copy the source items to the collection.

                        object sourceValue = sourceProperty.GetValue(fromObject, null);
                        object destinationValue = destinationProperty.GetValue(toObject, null);

                        CopyItems(sourceValue, destinationValue);

                    }
                    else if (destinationProperty.CanWrite && PropertyHasAccessibleSetter(destinationProperty) && destinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)) {

                        // Attempt to set the property directly.

                        object sourceValue = sourceProperty.GetValue(fromObject, null);

                        if (sourceValue is object || options.CopyNullValues)
                            destinationProperty.SetValue(toObject, sourceValue, null);

                    }

                }
                catch (Exception) {

                    if (!options.IgnoreExceptions)
                        throw;

                }

            }

        }
        public static void CopyItems(object source, object destination) {

            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (destination is null)
                throw new ArgumentNullException(nameof(destination));

            // Find a matching generic argument between the source IEnumerable and the destination ICollection.

            IEnumerable<Type> sourceTypes = GetInterfaceGenericArguments(source.GetType(), typeof(IEnumerable<>));

            if (!sourceTypes.Any())
                throw new ArgumentException(string.Format(Properties.ExceptionMessages.TypeDoesNotImplementInterfaceWithTypeNameAndInterfaceName, source.GetType().Name, typeof(IEnumerable<>).Name), nameof(source));

            IEnumerable<Type> destinationTypes = GetInterfaceGenericArguments(destination.GetType(), typeof(ICollection<>));

            if (!destinationTypes.Any())
                throw new ArgumentException(string.Format(Properties.ExceptionMessages.TypeDoesNotImplementInterfaceWithTypeNameAndInterfaceName, destination.GetType().Name, typeof(ICollection<>).Name), nameof(destination));

            IEnumerable<Type> matchingGenericArgumentTypes = sourceTypes
                .Where(type => destinationTypes.Contains(type));

            if (matchingGenericArgumentTypes.Count() != 1)
                throw new ArgumentException(string.Format(Properties.ExceptionMessages.NoSuitableInterfaceImplementationCouldBeDeterminedWithInterfaceName, typeof(ICollection<>).Name), nameof(destination));

            Type matchingGenericArgumentType = matchingGenericArgumentTypes.First();

            // Copy the items.

            MethodInfo addMethod = typeof(ICollection<>)
                .MakeGenericType(matchingGenericArgumentType)
                .GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { matchingGenericArgumentType }, null);

            foreach (object item in (IEnumerable)source)
                addMethod.Invoke(destination, new[] { item });

        }

        // Private members

        private static IEnumerable<Type> GetInterfaceGenericArguments(Type type, Type interfaceType) {

            return type.GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType)
                .Select(t => t.GetGenericArguments().First());

        }
        private static bool PropertyHasAccessibleSetter(PropertyInfo propertyInfo) {

            MethodInfo destinationPropertySetMethod = propertyInfo.GetSetMethod(nonPublic: true);

            return destinationPropertySetMethod is object &&
                !destinationPropertySetMethod.IsPrivate &&
                !destinationPropertySetMethod.Attributes.HasFlag(MethodAttributes.Static);

        }

    }

}