using System.Net;
using System.Net.Sockets;

namespace Gsemac.Net {

    public static class SocketUtilities {

        public static int GetAvailablePort() {

            TcpListener listener = new TcpListener(IPAddress.Any, 0);

            listener.Start();

            int port = ((IPEndPoint)listener.LocalEndpoint).Port;

            listener.Stop();

            return port;

        }
        public static bool IsPortAvailable(int port) {

            TcpListener listener = new TcpListener(IPAddress.Any, port);

            try {

                listener.Start();

                return true;

            }
            catch (SocketException ex) {

                if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    return false;

                throw ex;

            }
            finally {

                listener.Stop();

            }


        }

    }

}