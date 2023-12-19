using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Collections {

    public sealed class MappedCollectionDecorator<ItemT, SubItemT> :
        ICollection<SubItemT> {

        // Public members

        public int Count => underlyingCollection.Count;
        public bool IsReadOnly => underlyingCollection.IsReadOnly;

        public MappedCollectionDecorator(ICollection<ItemT> underlyingCollection, Func<ItemT, SubItemT> toSubItem, Func<SubItemT, ItemT> fromSubItem) {

            if (underlyingCollection is null)
                throw new ArgumentNullException(nameof(underlyingCollection));

            if (toSubItem is null)
                throw new ArgumentNullException(nameof(toSubItem));

            if (fromSubItem is null)
                throw new ArgumentNullException(nameof(fromSubItem));

            this.underlyingCollection = underlyingCollection;
            this.toSubItem = toSubItem;
            this.fromSubItem = fromSubItem;

        }

        public void Add(SubItemT item) {

            underlyingCollection.Add(fromSubItem(item));

        }
        public bool Contains(SubItemT item) {

            return underlyingCollection.Any(i => toSubItem(i)?.Equals(item) ?? false);

        }
        public bool Remove(SubItemT item) {

            ItemT itemToRemove = underlyingCollection.Where(i => toSubItem(i)?.Equals(item) ?? false)
                .FirstOrDefault();

            if (itemToRemove.Equals(default))
                return false;

            return underlyingCollection.Remove(itemToRemove);

        }

        public void Clear() {

            underlyingCollection.Clear();

        }

        public void CopyTo(SubItemT[] array, int arrayIndex) {

            underlyingCollection.Select(i => toSubItem(i))
                .ToArray()
                .CopyTo(array, arrayIndex);

        }

        public IEnumerator<SubItemT> GetEnumerator() {

            return (IEnumerator<SubItemT>)underlyingCollection.Select(i => toSubItem(i))
                .ToArray()
                .GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly ICollection<ItemT> underlyingCollection;
        private readonly Func<ItemT, SubItemT> toSubItem;
        private readonly Func<SubItemT, ItemT> fromSubItem;

    }

}