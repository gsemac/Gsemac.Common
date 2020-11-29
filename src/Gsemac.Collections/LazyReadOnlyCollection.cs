using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public class LazyReadOnlyCollection<T> :
        ICollection<T> {

        public int Count => items.Count();
        public bool IsReadOnly => true;

        public LazyReadOnlyCollection(IEnumerable<T> items) {

            this.items = items;

        }

        void ICollection<T>.Add(T item) {

            throw new NotSupportedException("Collection is read-only.");

        }
        void ICollection<T>.Clear() {

            throw new NotSupportedException("Collection is read-only.");

        }
        public bool Contains(T item) {

            return items.Any(i => i.Equals(item));

        }
        public void CopyTo(T[] array, int arrayIndex) {

            items.ToArray().CopyTo(array, arrayIndex);

        }
        bool ICollection<T>.Remove(T item) {

            throw new NotSupportedException("Collection is read-only.");

        }

        public IEnumerator<T> GetEnumerator() {

            return items.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly IEnumerable<T> items;

    }

}