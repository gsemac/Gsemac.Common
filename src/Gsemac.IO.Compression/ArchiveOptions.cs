using System;
using System.IO;
using System.Text;

namespace Gsemac.IO.Compression {

    public sealed class ArchiveOptions :
        IArchiveOptions {

        public string Comment { get; set; }
        public string Password { get; set; }
        public bool EncryptHeaders { get; set; } = false;
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Maximum;
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public bool LeaveStreamOpen { get; set; } = false;
        public FileAccess FileAccess { get; set; } = FileAccess.ReadWrite;

        public static ArchiveOptions Default => new ArchiveOptions();

        public ArchiveOptions() {
        }
        public ArchiveOptions(IArchiveOptions other) {

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            Comment = other.Comment;
            Password = other.Password;
            EncryptHeaders = other.EncryptHeaders;
            CompressionLevel = other.CompressionLevel;
            Encoding = other.Encoding;
            LeaveStreamOpen = other.LeaveStreamOpen;
            FileAccess = other.FileAccess;

        }

    }

}