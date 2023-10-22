namespace Gsemac.Net.Dns {

    public sealed class DnsQuestion :
        IDnsQuestion {

        // Public members

        public string Name { get; set; }
        public DnsRecordType RecordType { get; set; } = DnsRecordType.A;
        public DnsClass Class { get; set; } = DnsClass.Internet;

    }

}