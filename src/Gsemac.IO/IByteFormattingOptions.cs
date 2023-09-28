namespace Gsemac.IO {

    public interface IByteFormattingOptions {

        BinaryPrefix Prefix { get; }
        int Precision { get; }
        double Threshold { get; }

    }

}