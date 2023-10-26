using System;

namespace Gsemac.Net.Dns {

    public sealed class DnsQuestion :
        IDnsQuestion {

        // Public members

        public string Name { get; set; }
        public DnsRecordType RecordType { get; set; } = DnsRecordType.A;
        public DnsRecordClass RecordClass { get; set; } = DnsRecordClass.Internet;

        public DnsQuestion(string name) :
            this(name, DnsRecordType.A, DnsRecordClass.Internet) {
        }
        public DnsQuestion(string name, DnsRecordType recordType) :
            this(name, recordType, DnsRecordClass.Internet) {
        }
        public DnsQuestion(string name, DnsRecordType recordType, DnsRecordClass recordClass) {

            if (name is null)
                throw new ArgumentNullException(nameof(name));

            Name = name;
            RecordType = recordType;
            RecordClass = recordClass;

        }

    }

}