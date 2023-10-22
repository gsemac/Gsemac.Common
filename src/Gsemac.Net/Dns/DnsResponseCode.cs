namespace Gsemac.Net.Dns {

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

}