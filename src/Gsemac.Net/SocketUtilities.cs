using System.Net;
using System.Net.Sockets;

namespace Gsemac.Net {

    public static class SocketUtilities {

        public static int GetUnusedPort() {

            var listener = new TcpListener(IPAddress.Any, 0);

            listener.Start();

            var port = ((IPEndPoint)listener.LocalEndpoint).Port;

            listener.Stop();

            return port;

        }

    }

}