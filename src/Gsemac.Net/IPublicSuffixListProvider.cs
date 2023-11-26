using System.Collections.Generic;

namespace Gsemac.Net {

    public interface IPublicSuffixListProvider {

        IEnumerable<string> GetList();

    }

}