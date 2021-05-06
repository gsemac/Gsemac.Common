using System;
using System.Collections.Generic;

namespace Gsemac.Collections {

    public interface IConcurrentDictionary<TKey, TValue> :
        IDictionary<TKey, TValue> {

        bool TryAdd(TKey key, TValue value);
        bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue);
        TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory);
        TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory);
        TValue GetOrAdd(TKey key, TValue value);
        TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);

    }

}