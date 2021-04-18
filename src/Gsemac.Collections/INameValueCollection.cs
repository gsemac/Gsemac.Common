using System.Collections.Generic;

namespace Gsemac.Collections {

    public interface INameValueCollection :
        IDictionary<string, string> {

        IEnumerable<string> GetValues(string key);

    }

}