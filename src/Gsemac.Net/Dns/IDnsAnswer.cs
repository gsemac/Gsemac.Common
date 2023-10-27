using System;
using System.Net;

namespace Gsemac.Net.Dns {

    public interface IDnsAnswer {

        string Name { get; }
        DnsRecordType RecordType { get; }
        DnsRecordClass RecordClass { get; }
        TimeSpan TimeToLive { get; }

        string DomainName { get; }
        IPAddress HostAddress { get; }

    }

}