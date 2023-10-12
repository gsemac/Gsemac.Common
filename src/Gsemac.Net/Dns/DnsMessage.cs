using System.Collections.Generic;

namespace Gsemac.Net.Dns {

    public enum DnsMessageType {
        Query = 0,
        Response = 1,
    }

    // DNS CLASSes
    // https://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml#dns-parameters-2

    public enum DnsClass {
        Internet = 1,
        Chaos = 3,
        Hesiod = 4,
        None = 254,
        Any = 255,
    }

    // Resource Record (RR) TYPEs
    // https://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml#dns-parameters-4

    public enum DnsRecordType {
        A = 1,
        CName = 5,
    }

    // DNS OpCodes
    // https://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml#dns-parameters-5

    public enum DnsOpCode {
        Query = 0,
        IQuery = 1,
        Status = 2,
        Notify = 4,
        Update = 5,
        DnsStatefulOperations = 6,
    }

    // DNS RCODEs
    // https://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml#dns-parameters-6

    public enum DnsResponseCode {
        NoError = 0,
        FormatError = 1,
        ServerFailure = 2,
        NXDomain = 3,
        NotImplemented = 4,
        QueryRefused = 5,
        YXDomain = 6,
        YXRRSet = 7,
        NXRRSet = 8,
        NotAuthoritative = 9,
        NotAuthorized = 9,
        NotZone = 10,
        DsoTypeNotImplemented = 11,
        BadVersion = 16,
        BadSignature = 16,
        BadKey = 17,
        BadTime = 18,
        BadMode = 19,
        BadName = 20,
        BadAlgorithm = 21,
        BadTruncation = 22,
        BadCookie = 23,
    }

    public sealed class DnsMessage {

        // Public members

        public int Id { get; set; }
        public DnsMessageType Type { get; set; } = DnsMessageType.Query;
        public DnsOpCode OpCode { get; set; } = DnsOpCode.Query;
        public bool IsAuthoritativeAnswer { get; set; } = false;
        public bool IsTruncated { get; set; } = false;
        public bool RecursionDesired { get; set; } = true;
        public bool RecursionAvailable { get; set; } = false;
        public DnsResponseCode ResponseCode { get; set; } = DnsResponseCode.NoError;

        public ICollection<DnsQuestion> Questions { get; } = new List<DnsQuestion>();
        public ICollection<DnsAnswer> Answers { get; } = new List<DnsAnswer>();

    }

}