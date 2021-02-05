using System;

namespace Gsemac.Net {

    public delegate void DownloadFileCompletedEventHandler(object sender, DownloadFileCompletedEventArgs e);

    public class DownloadFileCompletedEventArgs :
        EventArgs {

        public bool Success { get; }
        public Uri Uri { get; }
        public string FilePath { get; }

        public DownloadFileCompletedEventArgs(Uri uri, string filePath, bool success) {

            Uri = uri;
            FilePath = filePath;
            Success = success;

        }

    }

}