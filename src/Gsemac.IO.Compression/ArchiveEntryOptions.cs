namespace Gsemac.IO.Compression {

    public class ArchiveEntryOptions :
        IArchiveEntryOptions {

        public string Comment { get; set; }
        public bool LeaveStreamOpen { get; set; } = false;
        public bool Overwrite { get; set; } = true;

        public static ArchiveEntryOptions Default => new ArchiveEntryOptions();

    }

}