using Gsemac.IO;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Gsemac.Net.Dns {

    public sealed class TcpDnsResolver :
        IDnsResolver {

        // Public members

        public TcpDnsResolver(IPEndPoint endpoint) :
            this(endpoint, TimeSpan.FromSeconds(5)) {
        }
        public TcpDnsResolver(IPEndPoint endpoint, TimeSpan timeout) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            this.endpoint = endpoint;
            this.timeout = timeout;

            serializer = new DnsMessageSerializer();

        }

        public IDnsMessage Resolve(IDnsMessage message) {

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            // For information on sending DNS over TCP:
            // https://datatracker.ietf.org/doc/html/rfc1035#section-4.2.2

            using (TcpClient tcp = new TcpClient(endpoint.AddressFamily)) {

                tcp.Client.SendTimeout = (int)timeout.TotalMilliseconds;
                tcp.Client.ReceiveTimeout = (int)timeout.TotalMilliseconds;

                tcp.Connect(endpoint);

                Stream tcpStream = tcp.GetStream();
                byte[] lengthBytes;

                using (MemoryStream requestStream = new MemoryStream()) {

                    serializer.Serialize(message, requestStream);

                    byte[] requestBytes = requestStream.ToArray();

                    // Prefix the message with the length of the message (the length of the prefix is not included).

                    lengthBytes = BitConverter.GetBytes((ushort)requestBytes.Length);

                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(lengthBytes);

                    tcpStream.Write(lengthBytes, 0, lengthBytes.Length);
                    tcpStream.Write(requestBytes, 0, requestBytes.Length);

                }

                tcpStream.Read(lengthBytes, 0, lengthBytes.Length);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lengthBytes);

                int responseLength = BitConverter.ToInt16(lengthBytes, 0);

                byte[] responseBytes = new byte[responseLength];

                tcpStream.Read(responseBytes, 0, responseBytes.Length);

                using (MemoryStream responseStream = new MemoryStream(responseBytes))
                    return serializer.Deserialize(responseStream);

            }

        }

        // Private members

        private readonly TimeSpan timeout;
        private readonly IDnsMessageSerializer serializer;
        private readonly IPEndPoint endpoint;

    }

}