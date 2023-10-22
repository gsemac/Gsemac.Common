namespace Gsemac.Net.Dns {

    // DNS CLASSes
    // https://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml#dns-parameters-2

    public enum DnsClass {
        Internet = 1,
        Chaos = 3,
        Hesiod = 4,
        None = 254,
        Any = 255,
    }

}