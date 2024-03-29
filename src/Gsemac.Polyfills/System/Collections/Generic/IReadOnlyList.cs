﻿namespace Gsemac.Polyfills.System.Collections.Generic {

    // IReadOnlyList<T> was added in .NET Framework 4.5

    /// <summary>
    /// Represents a read-only collection of elements that can be accessed by index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the read-only list.</typeparam>
    public interface IReadOnlyList<T> :
        IReadOnlyCollection<T> {

        /// <summary>
        /// Gets the element at the specified index in the read-only list.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the read-only list.</returns>
        T this[int index] { get; }

    } // .NET Framework 4.5 or later

}