using System;

namespace Gsemac.Net {

    public delegate void DownloadFileCompletedEventHandler(object sender, DownloadFileCompletedEventArgs e);

    public class DownloadFileCompletedEventArgs :
        EventArgs {

        public bool Success { get; }
        public string FilePath { get; }

        public DownloadFileCompletedEventArgs(string filePath, bool success) {

            FilePath = filePath;
            Success = success;

        }

    }

}