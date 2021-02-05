using System;

namespace Gsemac.Net {

    public delegate void DownloadProgressChangedEventHandler(object sender, DownloadProgressChangedEventArgs e);

    public class DownloadProgressChangedEventArgs :
          EventArgs {

        public long BytesReceived { get; }
        public long TotalBytesToReceive { get; }

        public DownloadProgressChangedEventArgs(long bytesReceived, long totalBytesToReceive) {

            BytesReceived = bytesReceived;
            TotalBytesToReceive = totalBytesToReceive;

        }
        public DownloadProgressChangedEventArgs(System.Net.DownloadProgressChangedEventArgs e) :
            this(e.BytesReceived, e.TotalBytesToReceive) {
        }

    }

}