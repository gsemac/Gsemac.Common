using System.Collections.Generic;

namespace Gsemac.Collections.Specialized {

    public interface INameValueCollection :
        IDictionary<string, string> {

        string Get(string key);

        IEnumerable<string> GetValues(string key);
        bool TryGetValues(string key, out IEnumerable<string> values);

        void Set(string key, string value);


    }

}