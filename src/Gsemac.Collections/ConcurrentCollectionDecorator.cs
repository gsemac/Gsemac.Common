using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public sealed class ConcurrentCollectionDecorator<T> :
        ICollection<T> {

        // Public members

        public int Count {
            get {

                lock (mutex)
                    return underlyingCollection.Count;

            }
        }
        public bool IsReadOnly {

            get {

                lock (mutex)
                    return underlyingCollection.IsReadOnly;

            }

        }

        public ConcurrentCollectionDecorator(ICollection<T> underlyingCollection) {

            if (underlyingCollection is null)
                throw new ArgumentNullException(nameof(underlyingCollection));

            this.underlyingCollection = underlyingCollection;

        }

        public void Add(T item) {

            lock (mutex)
                underlyingCollection.Add(item);

        }
        public bool Contains(T item) {

            lock (mutex)
                return underlyingCollection.Contains(item);

        }
        public bool Remove(T item) {

            lock (mutex)
                return underlyingCollection.Remove(item);

        }
        public void Clear() {

            lock (mutex)
                underlyingCollection.Clear();

        }

        public void CopyTo(T[] array, int arrayIndex) {

            lock (mutex)
                underlyingCollection.CopyTo(array, arrayIndex);

        }

        public IEnumerator<T> GetEnumerator() {

            // Return an enumerator for a "frozen" copy of the collection to avoid the possibly of it being modified while iterating.

            lock (mutex)
                return ((IEnumerable<T>)underlyingCollection.ToArray()).GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly ICollection<T> underlyingCollection;
        private readonly object mutex = new object();

    }

}