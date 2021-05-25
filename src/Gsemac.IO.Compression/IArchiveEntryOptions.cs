namespace Gsemac.IO.Compression {

    public interface IArchiveEntryOptions {

        string Comment { get; }
        bool LeaveStreamOpen { get; }
        bool Overwrite { get; }

    }

}