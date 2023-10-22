using System;
using System.Net;

namespace Gsemac.Net.Dns {

    public sealed class DnsAnswer :
        IDnsAnswer {

        // Public members

        public string Name { get; set; }
        public DnsRecordType RecordType { get; set; } = DnsRecordType.A;
        public DnsRecordClass Class { get; set; } = DnsRecordClass.Internet;
        public TimeSpan TimeToLive { get; set; } = TimeSpan.Zero;

        public IPAddress HostAddress { get; set; }

    }

}