using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Gsemac.Collections {

    public class LruDictionary<TKey, TValue> :
        IDictionary<TKey, TValue> {

        // Public members

        public int Capacity { get; set; }

        public ICollection<TKey> Keys => new LazyReadOnlyCollection<TKey>(values.Select(i => i.Key));
        public ICollection<TValue> Values => new LazyReadOnlyCollection<TValue>(values.Select(i => i.Value));

        public int Count => values.Count();
        public bool IsReadOnly => false;

        public TValue this[TKey key] {
            get => GetValue(key);
            set => SetValue(key, value, replace: true);
        }

        public LruDictionary(int capacity) {

            Capacity = capacity;

        }

        public bool ContainsKey(TKey key) {

            return nodeDict.ContainsKey(key);

        }
        public void Add(TKey key, TValue value) {

            SetValue(key, value, replace: false);

        }
        public bool Remove(TKey key) {

            if (nodeDict.TryGetValue(key, out LinkedListNode<KeyValuePair<TKey, TValue>> node)) {

                values.Remove(node);

                nodeDict.Remove(key);

                return true;

            }
            else {

                return false;

            }

        }
        public bool TryGetValue(TKey key, out TValue value) {

            if (nodeDict.TryGetValue(key, out LinkedListNode<KeyValuePair<TKey, TValue>> node)) {

                value = node.Value.Value;

                SetMostRecent(node);

                return true;

            }
            else {

                value = default;

                return false;

            }

        }
        public void Add(KeyValuePair<TKey, TValue> item) {

            SetValue(item.Key, item.Value, replace: false);

        }
        public void Clear() {

            values.Clear();

            nodeDict.Clear();

        }
        public bool Contains(KeyValuePair<TKey, TValue> item) {

            return nodeDict.ContainsKey(item.Key);

        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {

            values.Select(i => new KeyValuePair<TKey, TValue>(i.Key, i.Value)).ToArray()
                .CopyTo(array, arrayIndex);

        }
        public bool Remove(KeyValuePair<TKey, TValue> item) {

            return Remove(item.Key);

        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {

            return values.Select(i => new KeyValuePair<TKey, TValue>(i.Key, i.Value))
                .GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly LinkedList<KeyValuePair<TKey, TValue>> values = new LinkedList<KeyValuePair<TKey, TValue>>();
        private readonly IDictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> nodeDict = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>();

        private void SetMostRecent(LinkedListNode<KeyValuePair<TKey, TValue>> node) {

            values.Remove(node);

            values.AddFirst(node);

        }
        private void EvictOldest() {

            if (values.Any()) {

                LinkedListNode<KeyValuePair<TKey, TValue>> nodeToRemove = values.Last;

                bool removedFromDict = nodeDict.Remove(nodeToRemove.Value.Key);

                Debug.Assert(removedFromDict);

                values.Remove(nodeToRemove);

            }

        }

        private TValue GetValue(TKey key) {

            if (TryGetValue(key, out TValue value))
                return value;
            else
                throw new KeyNotFoundException(Properties.ExceptionMessages.KeyNotFound);

        }
        private void SetValue(TKey key, TValue value, bool replace) {

            if (!Remove(key) && Count >= Capacity)
                EvictOldest();

            if (!replace && ContainsKey(key))
                throw new ArgumentException(Properties.ExceptionMessages.KeyAlreadyExists, nameof(key));

            nodeDict[key] = values.AddFirst(new KeyValuePair<TKey, TValue>(key, value));

            Debug.Assert(Count <= Capacity);

        }

    }

}