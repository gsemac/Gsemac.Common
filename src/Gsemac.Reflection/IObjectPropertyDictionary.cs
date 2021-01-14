using System.Collections.Generic;

namespace Gsemac.Reflection {

    public interface IObjectPropertyDictionary :
        IDictionary<string, object> {

        bool TryGetValue<T>(string key, out T value);
        bool TrySetValue<T>(string key, T value);

    }

}