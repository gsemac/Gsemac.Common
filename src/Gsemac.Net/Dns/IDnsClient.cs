using System;
using System.Collections.Generic;
using System.Net;

namespace Gsemac.Net.Dns {

    public interface IDnsClient :
        IDisposable {

        /// <inheritdoc cref="System.Net.Dns.GetHostAddresses(string)"/>
        IEnumerable<IPAddress> GetHostAddresses(string hostNameOrAddress);
        /// <inheritdoc cref="System.Net.Dns.GetHostEntry(IPAddress)"/>
        IPHostEntry GetHostEntry(IPAddress address);
        /// <inheritdoc cref="System.Net.Dns.GetHostEntry(string)"/>
        IPHostEntry GetHostEntry(string hostNameOrAddress);
        /// <inheritdoc cref="System.Net.Dns.GetHostName"/>
        string GetHostName();

    }

}