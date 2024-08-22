using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Net {

    // Ideally, the public suffix list (https://publicsuffix.org/list/public_suffix_list.dat) should not be cached for an extended period of time.
    // To load the list from the file system instead (where it can be updated), use FileSystemPublicSuffixListProvider instead.

    internal sealed class ResourcePublicSuffixListProvider :
        PublicSuffixListProviderBase {

        // Public members

        public override IPublicSuffixList GetList() {

            return cache.Value;

        }

        // Private members

        // The same cache will be used for instances because it never changes.

        private static readonly Lazy<IPublicSuffixList> cache = new Lazy<IPublicSuffixList>(GetListInternal);

        private static IPublicSuffixList GetListInternal() {

            return new PublicSuffixList(ParseList(Encoding.UTF8.GetString(Properties.Resources.public_suffix_list)));

        }

    }

}