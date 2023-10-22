namespace Gsemac.Net.Dns {

    // Resource Record (RR) TYPEs
    // https://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml#dns-parameters-4

    public enum DnsRecordType {
        /// <summary>
        /// A host address.
        /// </summary>
        A = 1,
        /// <summary>
        /// An authoritative mail server.
        /// </summary>
        NS = 2,
        /// <summary>
        /// A Mail destination.
        /// </summary>
        MD = 3,
        /// <summary>
        /// A Mail forwarder.
        /// </summary>
        MF = 4,
        /// <summary>
        /// The canonical name for an alias.
        /// </summary>
        CNAME = 5,
    }

}