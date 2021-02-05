namespace Gsemac.Net {

    public delegate void DownloadFileProgressChangedEventHandler(object sender, DownloadFileProgressChangedEventArgs e);

    public class DownloadFileProgressChangedEventArgs :
        DownloadProgressChangedEventArgs {

        public string FilePath { get; }

        public DownloadFileProgressChangedEventArgs(string filePath, long bytesReceived, long totalBytesToReceive) :
            base(bytesReceived, totalBytesToReceive) {

            FilePath = filePath;

        }
        public DownloadFileProgressChangedEventArgs(string filePath, System.Net.DownloadProgressChangedEventArgs e) :
            base(e) {

            FilePath = filePath;

        }

    }

}