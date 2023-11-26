using System.Collections.Generic;
using System.Text;

namespace Gsemac.Net {

    // Ideally, the public suffix list (https://publicsuffix.org/list/public_suffix_list.dat) should not be cached for an extended period of time.
    // To load the list from the file system instead (where it can be updated), use FileSystemPublicSuffixListProvider instead.

    internal sealed class ResourcePublicSuffixListProvider :
        PublicSuffixListProviderBase {

        // Public members

        public override IEnumerable<string> GetList() {

            return ParseList(Encoding.UTF8.GetString(Properties.Resources.public_suffix_list));

        }

    }

}