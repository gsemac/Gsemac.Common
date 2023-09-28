namespace Gsemac.IO {

    public sealed class ByteFormattingOptions :
        IByteFormattingOptions {

        // Public members

        public BinaryPrefix Prefix { get; set; } = BinaryPrefix.Binary;
        public int Precision { get; set; } = 1;
        public double Threshold { get; set; } = 0.97;

        public static ByteFormattingOptions Default => new ByteFormattingOptions();

    }

}