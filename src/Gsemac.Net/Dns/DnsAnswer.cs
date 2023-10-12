using System;

namespace Gsemac.Net.Dns {

    public sealed class DnsAnswer {

        // Public members

        public string Name { get; set; }
        public DnsRecordType RecordType { get; set; } = DnsRecordType.A;
        public DnsClass Class { get; set; } = DnsClass.Internet;
        public TimeSpan Lifespan { get; set; } = TimeSpan.Zero;

    }

}