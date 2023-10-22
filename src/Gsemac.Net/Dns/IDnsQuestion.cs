namespace Gsemac.Net.Dns {

    public interface IDnsQuestion {

        string Name { get; }
        DnsRecordType RecordType { get; }
        DnsRecordClass Class { get; }

    }

}