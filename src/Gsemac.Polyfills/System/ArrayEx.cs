using System.Linq;

namespace System {

    /// <inheritdoc cref="Array"/>
    public static class ArrayEx {

        /// <summary>
        /// Returns an empty array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <returns>Returns an empty <see cref="Array"/>.</returns>
        public static T[] Empty<T>() {

            return Enumerable.Empty<T>().ToArray();

        } // .NET Framework 4.6 and later

    }

}