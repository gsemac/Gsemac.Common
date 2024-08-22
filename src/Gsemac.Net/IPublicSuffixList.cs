using System.Collections.Generic;

namespace Gsemac.Net {

    public interface IPublicSuffixList :
        IEnumerable<string> {

        bool Contains(string suffix);

    }

}