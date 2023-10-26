using Gsemac.Net.Properties;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Gsemac.Net.Dns {

    public sealed class UdpDnsResolver :
        IDnsResolver {

        // Public members

        public bool TcpFallbackEnabled { get; set; } = true;
        public TimeSpan Timeout { get; set; } = DefaultTimeout;

        public UdpDnsResolver(IPAddress endpoint) :
            this(endpoint, DefaultTimeout) {
        }
        public UdpDnsResolver(IPAddress endpoint, TimeSpan timeout) :
            this(new IPEndPoint(endpoint, DefaultPort), timeout) {
        }
        public UdpDnsResolver(IPEndPoint endpoint) :
            this(endpoint, DefaultTimeout) {
        }
        public UdpDnsResolver(IPEndPoint endpoint, TimeSpan timeout) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            this.endpoint = endpoint;
            Timeout = timeout;
            serializer = new DnsMessageSerializer();

        }

        public IDnsMessage Resolve(IDnsMessage message) {

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            using (UdpClient client = new UdpClient(endpoint.AddressFamily)) {

                client.Client.SendTimeout = (int)Timeout.TotalMilliseconds;
                client.Client.ReceiveTimeout = (int)Timeout.TotalMilliseconds;

                client.Connect(endpoint);

                using (MemoryStream requestStream = new MemoryStream()) {

                    serializer.Serialize(requestStream, message);

                    byte[] requestBytes = requestStream.ToArray();

                    client.Send(requestBytes, requestBytes.Length);

                }

                byte[] responseBytes;

                try {

                    responseBytes = client.Receive(ref endpoint);

                }
                catch (SocketException ex) {

                    // Timeouts are wrapped to make them easier to catch downstream.

                    if (ex.SocketErrorCode == SocketError.TimedOut)
                        throw new TimeoutException(ExceptionMessages.DnsRequestTimedOut, ex);

                    throw;

                }

                using (MemoryStream responseStream = new MemoryStream(responseBytes)) {

                    IDnsMessage responseMessage = serializer.Deserialize(responseStream);

                    // If the response is truncated, it was too large to transmit in a single UDP packet.
                    // Fall back to TCP to read the entire response message.

                    if (responseMessage is object && responseMessage.IsTruncated && TcpFallbackEnabled)
                        responseMessage = new TcpDnsResolver(endpoint, Timeout).Resolve(message);

                    return responseMessage;

                }

            }

        }

        // Private members

        private const int DefaultPort = 53;
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(2);

        private readonly IDnsMessageSerializer serializer;
        private IPEndPoint endpoint;

    }

}