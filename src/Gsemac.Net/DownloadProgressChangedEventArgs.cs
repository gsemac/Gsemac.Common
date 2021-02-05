using System;

namespace Gsemac.Net {

    public delegate void DownloadProgressChangedEventHandler(object sender, DownloadProgressChangedEventArgs e);

    public class DownloadProgressChangedEventArgs :
          EventArgs {

        public Uri Uri { get; }
        public long BytesReceived { get; }
        public long TotalBytesToReceive { get; }

        public DownloadProgressChangedEventArgs(Uri uri, long bytesReceived, long totalBytesToReceive) {

            Uri = uri;
            BytesReceived = bytesReceived;
            TotalBytesToReceive = totalBytesToReceive;

        }
        public DownloadProgressChangedEventArgs(Uri uri, System.Net.DownloadProgressChangedEventArgs e) :
            this(uri, e.BytesReceived, e.TotalBytesToReceive) {
        }

    }

}