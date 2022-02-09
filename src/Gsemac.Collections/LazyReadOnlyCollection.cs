using Gsemac.Collections.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public sealed class LazyReadOnlyCollection<T> :
        IList<T>,
        IReadOnlyList<T> {

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index] {
            get {

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index), ExceptionMessages.IndexOutOfRange);

                return items.ElementAt(index);

            }
        }

        T IList<T>.this[int index] {
            get => items.ElementAt(index);
            set => throw new NotSupportedException(ExceptionMessages.CollectionIsReadOnly);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="LazyReadOnlyCollection{T}"/> instance.
        /// </summary>
        public int Count => items.Count();

        bool ICollection<T>.IsReadOnly => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyReadOnlyCollection{T}"/> class that is a read-only wrapper around the specified <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="list">The list to wrap.</param>
        public LazyReadOnlyCollection(IEnumerable<T> items) {

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            this.items = items;

        }

        /// <summary>
        /// Determines whether an element is in the <see cref="LazyReadOnlyCollection{T}"./>
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="LazyReadOnlyCollection{T}"/>. The value can be <see cref="null"/> for reference types.</param>
        /// <returns><see cref="true"/> if value is found in the <see cref="LazyReadOnlyCollection{T}"/> otherwise, <see cref="false"/>.</returns>
        public bool Contains(T item) {

            return items.Any(i => i.Equals(item));

        }
        /// <summary>
        /// Copies the entire <see cref="LazyReadOnlyCollection{T}"/> to a compatible one-dimensional <see cref="Array"/>, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="LazyReadOnlyCollection{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex) {

            items.ToArray().CopyTo(array, arrayIndex);

        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="LazyReadOnlyCollection{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="LazyReadOnlyCollection{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator() {

            return items.GetEnumerator();

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

        // Private members

        private readonly IEnumerable<T> items;

    }

}