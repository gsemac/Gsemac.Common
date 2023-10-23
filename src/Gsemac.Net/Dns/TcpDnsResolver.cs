using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Gsemac.Net.Dns {

    public sealed class TcpDnsResolver :
        IDnsResolver {

        // Public members

        public TcpDnsResolver(IPEndPoint endpoint) :
            this(endpoint, DefaultTimeout) {
        }
        public TcpDnsResolver(IPEndPoint endpoint, SslProtocols sslProtocols) :
            this(endpoint, sslProtocols, DefaultTimeout) {
        }
        public TcpDnsResolver(IPEndPoint endpoint, TimeSpan timeout) :
            this(endpoint, DefaultSslProtocols, timeout) {
        }
        public TcpDnsResolver(IPEndPoint endpoint, SslProtocols sslProtocols, TimeSpan timeout) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            this.endpoint = endpoint;
            this.timeout = timeout;
            this.sslProtocols = sslProtocols;

            serializer = new DnsMessageSerializer();

        }

        public IDnsMessage Resolve(IDnsMessage message) {

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            // For information on sending DNS over TCP:
            // https://datatracker.ietf.org/doc/html/rfc1035#section-4.2.2

            using (TcpClient client = new TcpClient(endpoint.AddressFamily)) {

                client.Client.SendTimeout = (int)timeout.TotalMilliseconds;
                client.Client.ReceiveTimeout = (int)timeout.TotalMilliseconds;

                client.Connect(endpoint);

                Stream tcpStream = GetTcpStream(client);

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

                    if (tcpStream is SslStream)
                        tcpStream.Flush();

                }

                tcpStream.Read(lengthBytes, 0, lengthBytes.Length);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lengthBytes);

                int responseLength = BitConverter.ToInt16(lengthBytes, 0);

                byte[] responseBytes = new byte[responseLength];

                tcpStream.Read(responseBytes, 0, responseBytes.Length);

                if (tcpStream is SslStream)
                    tcpStream.Close();

                using (MemoryStream responseStream = new MemoryStream(responseBytes))
                    return serializer.Deserialize(responseStream);

            }

        }

        // Private members

        private readonly TimeSpan timeout;
        private readonly IDnsMessageSerializer serializer;
        private readonly IPEndPoint endpoint;
        private readonly SslProtocols sslProtocols;

        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
        private static readonly SslProtocols DefaultSslProtocols = SslProtocols.None;

        private Stream GetTcpStream(TcpClient client) {

            if (client is null)
                throw new ArgumentNullException(nameof(client));

            if (sslProtocols != SslProtocols.None) {

                SslStream stream = new SslStream(client.GetStream(), leaveInnerStreamOpen: false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

                try {

                    stream.AuthenticateAsClient(endpoint.Address.ToString(), null, sslProtocols, checkCertificateRevocation: true);

                }
                catch (Exception) {

                    // Certificate validation failed, so terminate the connection.

                    client.Close();

                }

                return stream;

            }
            else {

                return client.GetStream();

            }

        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {

            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            // Do not allow this client to communicate with unauthenticated servers.

            return false;
        }

    }

}