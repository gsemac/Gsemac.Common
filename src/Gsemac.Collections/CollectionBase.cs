using System;
using System.Collections;
using System.Collections.Generic;

namespace Gsemac.Collections {

    public abstract class CollectionBase<T> :
        ICollection<T> {

        // Public members

        public int Count => items.Count;
        public bool IsReadOnly => items.IsReadOnly;

        public void Add(T item) {

            items.Add(item);

        }
        public bool Remove(T item) {

            return items.Remove(item);

        }
        public void Clear() {

            items.Clear();

        }

        public bool Contains(T item) {

            return items.Contains(item);

        }

        public void CopyTo(T[] array, int arrayIndex) {

            items.CopyTo(array, arrayIndex);

        }

        public IEnumerator<T> GetEnumerator() {

            return items.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return items.GetEnumerator();

        }

        // Protected members

        protected CollectionBase() {

            items = new List<T>();

        }
        protected CollectionBase(ICollection<T> baseCollection) {

            if (baseCollection is null)
                throw new ArgumentNullException(nameof(baseCollection));

            items = baseCollection;

        }

        // Private members

        private readonly ICollection<T> items;

    }

}