namespace System.Net {

    // Tls11 and Tls12 were added in .NET Framework 4.5 (but are backwards-compatible .NET Framework 4.0).
    // SystemDefault was added in .NET Framework 4.7.
    // Tls13 was added in .NET Framework 4.8.

    /// <inheritdoc cref="SecurityProtocolType"/>
    [Flags]
    public enum SecurityProtocolTypeEx {
        /// <summary>
        /// Allows the operating system to choose the best protocol to use, and to block protocols that are not secure. Unless your app has a specific reason not to, you should use this value.
        /// </summary>
        SystemDefault = 0,
        /// <summary>
        /// Specifies the Secure Socket Layer (SSL) 3.0 security protocol. SSL 3.0 has been superseded by the Transport Layer Security (TLS) protocol and is provided for backward compatibility only.
        /// </summary>
        Ssl3 = 48,
        /// <summary>
        /// Specifies the Transport Layer Security (TLS) 1.0 security protocol. The TLS 1.0 protocol is defined in IETF RFC 2246.
        /// </summary>
        Tls = 192,
        /// <summary>
        /// Specifies the Transport Layer Security (TLS) 1.1 security protocol. The TLS 1.1 protocol is defined in IETF RFC 4346. On Windows systems, this value is supported starting with Windows 7.
        /// </summary>
        Tls11 = 768,
        /// <summary>
        /// Specifies the Transport Layer Security (TLS) 1.2 security protocol. The TLS 1.2 protocol is defined in IETF RFC 5246. On Windows systems, this value is supported starting with Windows 7.
        /// </summary>
        Tls12 = 3072,
        /// <summary>
        /// Specifies the TLS 1.3 security protocol. The TLS protocol is defined in IETF RFC 8446.
        /// </summary>
        Tls13 = 12288,
    }

}