using System;

namespace Gsemac.Net {

    public delegate void DownloadFileProgressChangedEventHandler(object sender, DownloadFileProgressChangedEventArgs e);

    public class DownloadFileProgressChangedEventArgs :
        DownloadProgressChangedEventArgs {

        public string Filename { get; }

        public DownloadFileProgressChangedEventArgs(Uri uri, string filePath, long bytesReceived, long totalBytesToReceive) :
            base(uri, bytesReceived, totalBytesToReceive) {

            Filename = filePath;

        }
        public DownloadFileProgressChangedEventArgs(Uri uri, string filePath, System.Net.DownloadProgressChangedEventArgs e) :
            base(uri, e) {

            Filename = filePath;

        }

    }

}