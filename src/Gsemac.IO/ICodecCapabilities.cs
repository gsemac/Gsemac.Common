namespace Gsemac.IO {

    public interface ICodecCapabilities {

        IFileFormat Format { get; }
        bool CanRead { get; }
        bool CanWrite { get; }

    }

}