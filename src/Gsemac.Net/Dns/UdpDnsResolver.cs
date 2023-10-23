using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Gsemac.Net.Dns {

    public sealed class UdpDnsResolver :
        IDnsResolver {

        // Public members

        public UdpDnsResolver(IPEndPoint endpoint) :
            this(endpoint, DefaultTimeout) {
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

            using (UdpClient client = new UdpClient(endpoint.AddressFamily)) {

                client.Client.SendTimeout = (int)timeout.TotalMilliseconds;
                client.Client.ReceiveTimeout = (int)timeout.TotalMilliseconds;

                client.Connect(endpoint);

                using (MemoryStream requestStream = new MemoryStream()) {

                    serializer.Serialize(requestStream, message);

                    byte[] requestBytes = requestStream.ToArray();

                    client.Send(requestBytes, requestBytes.Length);

                }

                byte[] responseBytes = client.Receive(ref endpoint);

                using (MemoryStream responseStream = new MemoryStream(responseBytes))
                    return serializer.Deserialize(responseStream);

            }

        }

        // Private members

        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(1);

        private readonly TimeSpan timeout;
        private readonly IDnsMessageSerializer serializer;
        private IPEndPoint endpoint;

    }

}