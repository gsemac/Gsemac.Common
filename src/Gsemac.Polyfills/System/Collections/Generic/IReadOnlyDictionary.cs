#if NET40_OR_LESSER

// IReadOnlyDictionary<TKey, TValue> was added in .NET Framework 4.5

namespace System.Collections.Generic {

    /// <summary>
    /// Represents a generic read-only collection of key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the read-only dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the read-only dictionary.</typeparam>
    public interface IReadOnlyDictionary<TKey, TValue> :
        IReadOnlyCollection<KeyValuePair<TKey, TValue>> {

        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        TValue this[TKey key] { get; }

        /// <summary>
        /// Gets an enumerable collection that contains the keys in the read-only dictionary.
        /// </summary>
        IEnumerable<TKey> Keys { get; }
        /// <summary>
        /// Gets an enumerable collection that contains the values in the read-only dictionary.
        /// </summary>
        IEnumerable<TValue> Values { get; }

        /// <summary>
        /// Determines whether the read-only dictionary contains an element that has the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns><see langword="true"/> if the read-only dictionary contains an element that has the specified key; otherwise, <see langword="false"/>.</returns>
        bool ContainsKey(TKey key);
        /// <summary>
        /// Gets the value that is associated with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the object that implements the <see cref="IReadOnlyDictionary{TKey, TValue}"/> interface contains an element that has the specified key; otherwise, <see langword="false"/>.</returns>
        bool TryGetValue(TKey key, out TValue value);

    }

}

#endif