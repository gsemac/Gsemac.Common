using System;

namespace Gsemac.Net {

    public delegate void DownloadFileCompletedEventHandler(object sender, DownloadFileCompletedEventArgs e);

    public class DownloadFileCompletedEventArgs :
        EventArgs {

        public bool Success { get; }
        public Uri Uri { get; }
        public string Filename { get; }

        public DownloadFileCompletedEventArgs(Uri uri, string filename, bool success) {

            Uri = uri;
            Filename = filename;
            Success = success;

        }

    }

}