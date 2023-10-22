using System.Collections.Generic;

namespace Gsemac.Net.Dns {

    public interface IDnsMessage {

        int Id { get; }
        DnsMessageType Type { get; }
        DnsOpCode OpCode { get; }
        bool IsAuthoritativeAnswer { get; }
        bool IsTruncated { get; }
        bool RecursionDesired { get; }
        bool RecursionAvailable { get; }
        DnsResponseCode ResponseCode { get; }

        ICollection<IDnsAnswer> Answers { get; }
        ICollection<IDnsQuestion> Questions { get; }

    }

}