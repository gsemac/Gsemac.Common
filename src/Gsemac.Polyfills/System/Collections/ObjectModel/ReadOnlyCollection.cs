using Gsemac.Polyfills.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Polyfills.System.Collections.ObjectModel {

    // .NET 4.0's ReadOnlyCollection<T> implementation doesn't implement IReadOnlyList<T> because the interface didn't exist until .NET 4.5.

    /// <summary>
    /// Provides the base class for a generic read-only collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ReadOnlyCollection<T> :
        IList<T>,
        Generic.IReadOnlyList<T> {

        // Public members

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index] {
            get {

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index), ExceptionMessages.IndexOutOfRange);

                return list[index];

            }
        }

        T IList<T>.this[int index] {
            get => list[index];
            set => throw new NotSupportedException(ExceptionMessages.CollectionIsReadOnly);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ReadOnlyCollection{T}"/> instance.
        /// </summary>
        public int Count => list.Count;

        bool ICollection<T>.IsReadOnly => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyCollection{T}"/> class that is a read-only wrapper around the specified list.
        /// </summary>
        /// <param name="list">The list to wrap.</param>
        public ReadOnlyCollection(IList<T> list) {

            if (list is null)
                throw new ArgumentNullException(nameof(list));

            this.list = list;

        }

        /// <summary>
        /// Determines whether an element is in the <see cref="ReadOnlyCollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ReadOnlyCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if value is found in the <see cref="ReadOnlyCollection{T}"/> otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item) {

            return list.Contains(item);

        }
        /// <summary>
        /// Copies the entire <see cref="ReadOnlyCollection{T}"/> to a compatible one-dimensional <see cref="ArrayEx"/>, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="ArrayEx"/> that is the destination of the elements copied from <see cref="ReadOnlyCollection{T}"/>. The <see cref="ArrayEx"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex) {

            list.CopyTo(array, arrayIndex);

        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ReadOnlyCollection{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="ReadOnlyCollection{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator() {

            return list.GetEnumerator();

        }

        void ICollection<T>.Add(T item) => throw new NotSupportedException(ExceptionMessages.CollectionIsReadOnly);
        void ICollection<T>.Clear() => throw new NotSupportedException(ExceptionMessages.CollectionIsReadOnly);
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException(ExceptionMessages.CollectionIsReadOnly);
        int IList<T>.IndexOf(T item) => throw new NotSupportedException(ExceptionMessages.CollectionIsReadOnly);
        void IList<T>.Insert(int index, T item) => throw new NotSupportedException(ExceptionMessages.CollectionIsReadOnly);
        void IList<T>.RemoveAt(int index) => throw new NotSupportedException(ExceptionMessages.CollectionIsReadOnly);

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Protected members

        /// <summary>
        /// Returns the <see cref="IList{T}"/> that the <see cref="ReadOnlyCollection{T}"/> wraps.
        /// </summary>
        protected IList<T> Items { get; }

        // Private members

        private readonly IList<T> list;

    }

}