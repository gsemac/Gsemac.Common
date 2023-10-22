using System.Collections.Generic;

namespace Gsemac.Net.Dns {

    public sealed class DnsMessage :
        IDnsMessage {

        // Public members

        public int Id { get; set; }
        public DnsMessageType Type { get; set; } = DnsMessageType.Query;
        public DnsOpCode OpCode { get; set; } = DnsOpCode.Query;
        public bool IsAuthoritativeAnswer { get; set; } = false;
        public bool IsTruncated { get; set; } = false;
        public bool RecursionDesired { get; set; } = true;
        public bool RecursionAvailable { get; set; } = false;
        public DnsResponseCode ResponseCode { get; set; } = DnsResponseCode.NoError;

        public ICollection<IDnsQuestion> Questions { get; } = new List<IDnsQuestion>();
        public ICollection<IDnsAnswer> Answers { get; } = new List<IDnsAnswer>();

    }

}