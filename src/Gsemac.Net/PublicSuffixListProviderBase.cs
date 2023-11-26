using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net {

    public abstract class PublicSuffixListProviderBase :
        IPublicSuffixListProvider {

        // Public members

        public virtual IEnumerable<string> GetList() {

            return new ResourcePublicSuffixListProvider().GetList();

        }

        // Protected members

        protected static IEnumerable<string> ParseList(string list) {

            if (string.IsNullOrWhiteSpace(list))
                return Enumerable.Empty<string>();

            IEnumerable<string> suffixes = list
                .Split(new[] { '\n' }, StringSplitOptions.None)
                .Where(x => !string.IsNullOrWhiteSpace(x) && !x.StartsWith("//"))
                .Select(x => '.' + x.TrimStart('*', '!', '.').Trim())
                .OrderByDescending(x => x.Length);

            return suffixes;

        }

    }

}