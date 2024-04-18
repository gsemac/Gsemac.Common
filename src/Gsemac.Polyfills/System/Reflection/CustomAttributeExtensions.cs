using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gsemac.Polyfills.System.Reflection {

    // The following extension methods were added in .NET Framework 4.5.
    // https://learn.microsoft.com/en-us/dotnet/api/system.reflection.customattributeextensions?view=netframework-4.5

    public static class CustomAttributeExtensions {

        // Public members

        public static Attribute GetCustomAttribute(this MemberInfo element, Type attributeType) {

            return GetCustomAttribute(element, attributeType, inherit: false);

        }
        public static Attribute GetCustomAttribute(this MemberInfo element, Type attributeType, bool inherit) {

            return GetAttributesOfType(element, attributeType, requireSingleAttribute: true, inherit: inherit)
                .FirstOrDefault();

        }
        public static Attribute GetCustomAttribute(this ParameterInfo element, Type attributeType) {

            return GetCustomAttribute(element, attributeType, inherit: false);

        }
        public static Attribute GetCustomAttribute(this ParameterInfo element, Type attributeType, bool inherit) {

            return GetAttributesOfType(element, attributeType, requireSingleAttribute: true, inherit: inherit)
                   .FirstOrDefault();

        }

        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, Type attributeType) {

            return GetAttributesOfType(element, attributeType, requireSingleAttribute: false, inherit: false);

        }
        public static IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, Type attributeType) {

            return GetAttributesOfType(element, attributeType, requireSingleAttribute: true, inherit: false);

        }

        public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute {

            return (T)GetCustomAttribute(element, typeof(T));

        }
        public static T GetCustomAttribute<T>(this MemberInfo element, bool inherit) where T : Attribute {

            return (T)GetCustomAttribute(element, typeof(T), inherit);

        }
        public static T GetCustomAttribute<T>(this ParameterInfo element) where T : Attribute {

            return (T)GetCustomAttribute(element, typeof(T));

        }
        public static T GetCustomAttribute<T>(this ParameterInfo element, bool inherit) where T : Attribute {

            return (T)GetCustomAttribute(element, typeof(T), inherit);

        }

        public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element) {

            return GetAttributesOfType(element, typeof(T), requireSingleAttribute: false, inherit: false)
                .Cast<T>();

        }
        public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element, bool inherit) {

            return GetAttributesOfType(element, typeof(T), requireSingleAttribute: false, inherit: inherit)
                   .Cast<T>();

        }
        public static IEnumerable<T> GetCustomAttributes<T>(this ParameterInfo element) {

            return GetAttributesOfType(element, typeof(T), requireSingleAttribute: false, inherit: false)
                   .Cast<T>();

        }
        public static IEnumerable<T> GetCustomAttributes<T>(this ParameterInfo element, bool inherit) {

            return GetAttributesOfType(element, typeof(T), requireSingleAttribute: false, inherit: inherit)
                   .Cast<T>();

        }

        // Private members

        private static IEnumerable<Attribute> GetAttributesOfType(ICustomAttributeProvider element, Type attributeType, bool requireSingleAttribute, bool inherit) {

            if (element is null)
                throw new ArgumentNullException(nameof(element));

            if (attributeType is null)
                throw new ArgumentNullException(nameof(attributeType));

            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException($"{nameof(attributeType)} is not derived from Attribute.", nameof(attributeType));

            var matchingAttributes = element.GetCustomAttributes(inherit: inherit)
                .Where(attribute => attribute.GetType() == attributeType);

            if (requireSingleAttribute && matchingAttributes.Skip(1).Any())
                throw new AmbiguousMatchException("More than one of the requested attributes was found.");

            return matchingAttributes.Cast<Attribute>();

        }

    }

}