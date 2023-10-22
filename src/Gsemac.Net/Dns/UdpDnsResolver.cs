using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Gsemac.Net.Dns {

    public sealed class UdpDnsResolver :
        IDnsResolver {

        // Public members

        public UdpDnsResolver(IPEndPoint endpoint) :
            this(endpoint, TimeSpan.FromSeconds(1)) {
        }
        public UdpDnsResolver(IPEndPoint endpoint, TimeSpan timeout) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            this.endpoint = endpoint;
            this.timeout = timeout;

            serializer = new DnsMessageSerializer();

        }

        public IDnsMessage Resolve(IDnsMessage message) {

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            using (UdpClient udp = new UdpClient(endpoint.AddressFamily)) {

                udp.Client.SendTimeout = (int)timeout.TotalMilliseconds;
                udp.Client.ReceiveTimeout = (int)timeout.TotalMilliseconds;

                udp.Connect(endpoint);

                using (MemoryStream requestStream = new MemoryStream()) {

                    serializer.Serialize(message, requestStream);

                    byte[] requestBytes = requestStream.ToArray();

                    udp.Send(requestBytes, requestBytes.Length);

                }

                byte[] responseBytes = udp.Receive(ref endpoint);

                using (MemoryStream responseStream = new MemoryStream(responseBytes))
                    return serializer.Deserialize(responseStream);

            }

        }

        // Private members

        private readonly TimeSpan timeout;
        private readonly IDnsMessageSerializer serializer;
        private IPEndPoint endpoint;

    }

}