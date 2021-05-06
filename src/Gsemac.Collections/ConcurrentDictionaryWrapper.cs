using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Gsemac.Collections {

    // This class uses ReaderWriterLockSlim to synchronize access to the underlying dictionary.
    // However, it should be noted that not all IDictionary implementations feature threadsafe reads (although the default Dictionary class does).
    // The relevant constructor parameter can be used to enable/disable concurrent reads.

    public class ConcurrentDictionaryWrapper<TKey, TValue> :
        IConcurrentDictionary<TKey, TValue> {

        // Public members

        public TValue this[TKey key] {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public ICollection<TKey> Keys => new LazyReadOnlyCollection<TKey>(GetKeyValuePairs().Select(p => p.Key));
        public ICollection<TValue> Values => new LazyReadOnlyCollection<TValue>(GetKeyValuePairs().Select(p => p.Value));
        public int Count {
            get {

                EnterReadLock();

                try {

                    return underlyingDictionary.Count;

                }
                finally {

                    ExitReadLock();

                }

            }
        }
        public bool IsReadOnly {
            get {

                EnterReadLock();

                try {

                    return underlyingDictionary.IsReadOnly;

                }
                finally {

                    ExitReadLock();

                }

            }
        }

        public ConcurrentDictionaryWrapper(IDictionary<TKey, TValue> underlyingDictionary, bool allowConcurrentReads = false) {

            this.underlyingDictionary = underlyingDictionary;
            this.allowConcurrentReads = allowConcurrentReads;

        }

        public void Add(TKey key, TValue value) {

            EnterWriteLock();

            try {

                underlyingDictionary.Add(key, value);

            }
            finally {

                ExitWriteLock();

            }

        }
        public void Add(KeyValuePair<TKey, TValue> item) {

            EnterWriteLock();

            try {

                underlyingDictionary.Add(item);

            }
            finally {

                ExitWriteLock();

            }

        }
        public bool TryGetValue(TKey key, out TValue value) {

            EnterReadLock();

            try {

                return underlyingDictionary.TryGetValue(key, out value);

            }
            finally {

                ExitReadLock();

            }

        }
        public void Clear() {

            EnterWriteLock();

            try {

                underlyingDictionary.Clear();

            }
            finally {

                ExitWriteLock();

            }

        }
        public bool Contains(KeyValuePair<TKey, TValue> item) {

            EnterReadLock();

            try {

                return underlyingDictionary.Contains(item);

            }
            finally {

                ExitReadLock();

            }

        }
        public bool ContainsKey(TKey key) {

            EnterReadLock();

            try {

                return underlyingDictionary.ContainsKey(key);

            }
            finally {

                ExitReadLock();

            }

        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {

            EnterReadLock();

            try {

                underlyingDictionary.CopyTo(array, arrayIndex);

            }
            finally {

                ExitReadLock();

            }

        }
        public bool Remove(TKey key) {

            mutex.EnterWriteLock();

            try {

                return underlyingDictionary.Remove(key);

            }
            finally {

                mutex.ExitWriteLock();

            }

        }
        public bool Remove(KeyValuePair<TKey, TValue> item) {

            mutex.EnterWriteLock();

            try {

                return underlyingDictionary.Remove(item);

            }
            finally {

                mutex.ExitWriteLock();

            }

        }

        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory) {

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (addValueFactory is null)
                throw new ArgumentNullException(nameof(addValueFactory));

            if (updateValueFactory is null)
                throw new ArgumentNullException(nameof(updateValueFactory));

            EnterWriteLock();

            try {

                if (underlyingDictionary.TryGetValue(key, out TValue value)) {

                    TValue newValue = updateValueFactory(key, value);

                    underlyingDictionary[key] = newValue;

                    return newValue;

                }
                else {

                    TValue newValue = addValueFactory(key);

                    underlyingDictionary[key] = newValue;

                    return newValue;

                }

            }
            finally {

                ExitWriteLock();

            }

        }
        public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory) {

            return AddOrUpdate(key, k => addValue, updateValueFactory);

        }
        public TValue GetOrAdd(TKey key, TValue value) {

            return GetOrAdd(key, k => value);

        }
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory) {

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (valueFactory is null)
                throw new ArgumentNullException(nameof(valueFactory));

            EnterReadLock(upgradeable: true);

            try {

                if (underlyingDictionary.TryGetValue(key, out TValue value))
                    return value;

                TValue newValue = valueFactory(key);

                try {

                    if (allowConcurrentReads)
                        EnterWriteLock();

                    underlyingDictionary.Add(key, newValue);

                }
                finally {

                    if (allowConcurrentReads)
                        ExitWriteLock();

                }

                return newValue;

            }
            finally {

                ExitReadLock(upgradeable: true);

            }

        }
        public bool TryAdd(TKey key, TValue value) {

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            EnterWriteLock();

            try {

                if (underlyingDictionary.ContainsKey(key))
                    return false;

                underlyingDictionary.Add(key, value);

                return true;

            }
            finally {

                ExitWriteLock();

            }

        }
        public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue) {

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            EnterWriteLock();

            try {

                if (underlyingDictionary.TryGetValue(key, out TValue value) && value.Equals(comparisonValue)) {

                    underlyingDictionary[key] = newValue;

                    return true;

                }
                else {

                    return false;

                }

            }
            finally {

                ExitWriteLock();

            }

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {

            return GetKeyValuePairs().GetEnumerator();

        }

        // Private members

        private readonly IDictionary<TKey, TValue> underlyingDictionary;
        private readonly bool allowConcurrentReads;
        private readonly ReaderWriterLockSlim mutex = new ReaderWriterLockSlim();

        private void EnterReadLock(bool upgradeable = false) {

            if (allowConcurrentReads) {

                if (upgradeable)
                    mutex.EnterUpgradeableReadLock();
                else
                    mutex.EnterReadLock();

            }
            else {

                EnterWriteLock();

            }

        }
        private void ExitReadLock(bool upgradeable = false) {

            if (allowConcurrentReads) {

                if (upgradeable)
                    mutex.ExitUpgradeableReadLock();
                else
                    mutex.ExitReadLock();

            }
            else {

                ExitWriteLock();

            }

        }
        private void EnterWriteLock() {

            mutex.EnterWriteLock();

        }
        private void ExitWriteLock() {

            mutex.ExitWriteLock();

        }

        private void SetValue(TKey key, TValue value) {

            EnterWriteLock();

            try {

                underlyingDictionary[key] = value;

            }
            finally {

                ExitWriteLock();

            }

        }
        private TValue GetValue(TKey key) {

            EnterReadLock();

            try {

                return underlyingDictionary[key];

            }
            finally {

                ExitReadLock();

            }

        }

        private ICollection<KeyValuePair<TKey, TValue>> GetKeyValuePairs() {

            EnterReadLock();


            try {

                return new ReadOnlyCollection<KeyValuePair<TKey, TValue>>(underlyingDictionary.ToList());

            }
            finally {

                ExitReadLock();

            }

        }

    }

}