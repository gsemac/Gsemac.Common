using System.Reflection;

namespace Gsemac.Polyfills.System.Reflection {

    public static class PropertyInfoExtensions {

        // GetValue and SetValue were added in .NET Framework 4.5.

        /// <summary>
        /// Returns the property value of a specified object.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <returns>The property value of the specified object.</returns>
        public static object GetValue(this PropertyInfo propertyInfo, object obj) {

            return propertyInfo.GetValue(obj, null);

        }
        /// <summary>
        /// Sets the property value of a specified object.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        public static void SetValue(this PropertyInfo propertyInfo, object obj, object value) {

            propertyInfo.SetValue(obj, value, null);

        }

    }

}