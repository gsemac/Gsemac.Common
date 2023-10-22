namespace Gsemac.Net.Dns {

    public sealed class DnsQuestion :
        IDnsQuestion {

        // Public members

        public string Name { get; set; }
        public DnsRecordType RecordType { get; set; } = DnsRecordType.A;
        public DnsRecordClass Class { get; set; } = DnsRecordClass.Internet;

    }

}