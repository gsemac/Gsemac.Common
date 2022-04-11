using System.Collections.Generic;

namespace Gsemac.Polyfills.System.Collections.Generic {

    // IReadOnlyCollection<T> was added in .NET Framework 4.5

    /// <summary>
    ///  Represents a strongly-typed, read-only collection of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public interface IReadOnlyCollection<T> :
        IEnumerable<T> {

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        int Count { get; }

    }

}