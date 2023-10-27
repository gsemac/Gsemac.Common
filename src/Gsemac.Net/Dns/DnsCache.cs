using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.Dns {

    internal sealed class DnsCache :
        IDnsCache {

        // Public members

        public bool TryGetRecords(string name, out IDnsAnswer[] records) {

            records = null;

            lock (mutex) {

                if (!cache.TryGetValue(name, out var cachedRecords))
                    return false;

                records = cachedRecords.Where(r => !r.IsExpired)
                    .Select(r => r.Record)
                    .ToArray();

            }

            return records is object &&
                records.Any();

        }
        public void AddRecords(string name, IDnsAnswer[] records) {

            if (records is null)
                throw new ArgumentNullException(nameof(records));

            var recordInfos = records
                .Select(r => CreateRecordInfo(r));

            lock (mutex) {

                if (cache.TryGetValue(name, out var cachedRecords)) {

                    // Before adding the new records, remove any existing records of the same type, as well as expired records.

                    HashSet<DnsRecordType> recordTypesToRemove = new HashSet<DnsRecordType>(records.Select(r => r.RecordType));

                    cachedRecords.RemoveAll(r => r.IsExpired || recordTypesToRemove.Contains(r.Record.RecordType));

                    // Add the new records to the cache.

                    cachedRecords.AddRange(recordInfos);

                }
                else {

                    cache[name] = new List<RecordInfo>(recordInfos);

                }

            }


        }

        // Private members

        private sealed class RecordInfo {

            // Public members

            public IDnsAnswer Record { get; }
            public DateTimeOffset Expiration { get; }
            public bool IsExpired => Expiration < DateTimeOffset.Now;

            public RecordInfo(IDnsAnswer record, DateTimeOffset expiration) {

                if (record is null)
                    throw new ArgumentNullException(nameof(record));

                Record = record;
                Expiration = expiration;

            }

        }

        private readonly object mutex = new object();
        private readonly IDictionary<string, List<RecordInfo>> cache = new Dictionary<string, List<RecordInfo>>();

        private static RecordInfo CreateRecordInfo(IDnsAnswer record) {

            if (record is null)
                throw new ArgumentNullException(nameof(record));

            return new RecordInfo(record, DateTimeOffset.Now + record.TimeToLive);

        }

    }

}