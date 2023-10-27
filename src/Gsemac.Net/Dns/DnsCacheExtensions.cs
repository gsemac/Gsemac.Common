using System;
using System.Linq;

namespace Gsemac.Net.Dns {

    internal static class DnsCacheExtensions {

        // Public members

        public static bool TryGetRecords(this IDnsCache dnsCache, IDnsQuestion query, out IDnsAnswer[] records) {

            if (dnsCache is null)
                throw new ArgumentNullException(nameof(dnsCache));

            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (dnsCache.TryGetRecords(query.Name, out records)) {

                records = records
                    .Where(r => r.RecordType == query.RecordType && r.RecordClass == query.RecordClass)
                    .ToArray();

                return records.Any();

            }

            return false;

        }

    }

}