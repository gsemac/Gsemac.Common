namespace Gsemac.Net.Dns {

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

}