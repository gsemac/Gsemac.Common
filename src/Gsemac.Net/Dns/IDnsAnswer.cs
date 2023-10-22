using System;
using System.Net;

namespace Gsemac.Net.Dns {

    public interface IDnsAnswer {

        string Name { get; }
        DnsRecordType RecordType { get; }
        DnsRecordClass Class { get; }
        TimeSpan TimeToLive { get; }

        IPAddress HostAddress { get; }

    }

}